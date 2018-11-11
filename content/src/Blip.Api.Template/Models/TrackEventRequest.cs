using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blip.Api.Template.Models
{
    /// <summary>
    /// Model to be used to request external link event tracking with Extras
    /// </summary>
    public class TrackEventRequest
    {
        /// <summary>
        /// contact.identity of the user to be tracked
        /// </summary>
        public string identity { get; set; }

        /// <summary>
        /// Action of the event to be tracked
        /// </summary>
        public string action { get; set; }

        /// <summary>
        /// Category of the event to be tracked
        /// </summary>
        public string category { get; set; }

        /// <summary>
        /// Dictionary of extras to be added to the tracked event
        /// </summary>
        public Dictionary<string, string> extras { get; set; }

        /// <summary>
        /// URL to redirect the user to
        /// </summary>
        public string redirect { get; set; }
    }
}
