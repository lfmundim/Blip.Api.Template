using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blip.Api.Template.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Take.Blip.Client.Extensions.EventTracker;

namespace Blip.Api.Template.Controllers
{
    /// <summary>
    /// Controller to track external links clicks
    /// </summary>
    [Produces("application/json")]
    [Route("api/Tracking")]
    public class TrackingController : Controller
    {
        private readonly IEventTrackExtension _eventTrack;

        public TrackingController(IEventTrackExtension eventTrack)
        {
            _eventTrack = eventTrack;
        }

        /// <summary>
        /// Endpoint to track clicks on external URL's links and redirect to them.
        /// </summary>
        /// <param name="identity">{{contact.identity}}</param>
        /// <param name="category">The category of the event</param>
        /// <param name="action">The action of the event</param>
        /// <param name="redirect">URL to be redirected to</param>
        /// <returns></returns>
        [HttpGet, Route("track/{identity}")]
        public async Task<IActionResult> TrackAndRedirectAsync(
            string identity,
            [FromQuery(Name = "category")]string category,
            [FromQuery(Name = "action")]string action,
            [FromQuery(Name = "redirect")]string redirect)
        {
            await _eventTrack.AddAsync(category, action, identity: identity);
            if (!redirect.StartsWith("https://") && !redirect.StartsWith("http://"))
            {
                redirect = "https://" + redirect;
            }
            return Redirect(redirect);
        }

        /// <summary>
        /// Endpoint to track with extras clicks on external URL's links and redirect to them
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("track")]
        public async Task<IActionResult> TrackAndRedirectAsync([FromBody] TrackEventRequest request)
        {
            await _eventTrack.AddAsync(request.category, request.action, request.extras, identity: request.identity);
            return Redirect(request.redirect);
        }
    }
}