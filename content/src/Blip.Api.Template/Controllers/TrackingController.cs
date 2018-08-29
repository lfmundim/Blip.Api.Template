using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Lime.Protocol;
using Microsoft.AspNetCore.Mvc;
using Take.Blip.Client;
using Take.Blip.Client.Extensions.EventTracker;
using Takenet.Iris.Messaging.Resources;

namespace Blip.Api.Template.Controllers
{
    [Route("api/[controller]")]
    public class TrackingController : Controller
    {
        private readonly IEventTrackExtension _eventTrack;
        private readonly ISender _sender;

        /// <summary>
        /// Controller aimed to  do special kinds of tracking (such as external link clicks)
        /// </summary>
        /// <param name="eventTrack"></param>
        /// <param name="sender"></param>
        public TrackingController(IEventTrackExtension eventTrack, ISender sender)
        {
            _eventTrack = eventTrack;
            _sender = sender;
        }

        /// <summary>
        /// Tracks a specific Category/Action pair and redirects user to the previously Encoded+Converted BASE64 url
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET api/Tracking/External%20Links/Google%20Link/d3d3Lmdvb2dsZS5jb20=
        /// </remarks>
        /// <param name="eventCategory"></param>
        /// <param name="eventAction"></param>
        /// <param name="encodedRedirect"></param>
        /// <returns></returns>
        [HttpGet, Route("{eventCategory}/{eventAction}/{encodedRedirect}")]
        public async Task<IActionResult> ExternalLinkTracking(string eventCategory, string eventAction, string encodedRedirect)
        {
            var decoded = "https://"+HttpUtility.UrlDecode(encodedRedirect);
            var decodedUri = new Uri(decoded);

            var redirect = new RedirectResult(decodedUri.AbsoluteUri);

            var trackCommand = new Command
            {
                To = "postmaster@msging.net",
                Id = EnvelopeId.NewId(),
                Uri = new LimeUri("/event-track"),
                Method = CommandMethod.Set,
                Resource = new EventTrack
                {
                    Category = eventCategory,
                    Action = eventAction
                }
            };
            await _sender.SendCommandAsync(trackCommand, CancellationToken.None);

            return redirect;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
    }
}
