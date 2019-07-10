using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Blip.Api.Template
{
    /// <summary>
    /// Class to use data from appsettings.json "Settings" field
    /// </summary>
    public class MySettings
    {
        /// <summary>
        /// BLiP's bot identifier and access key
        /// </summary>
        [JsonProperty("BlipBotSettings")]
        public BlipBotSettings BlipBotSettings { get; set; }

        [JsonProperty("ContentProvider")]
        public ContentProviderSettings ContentProvider { get; set; }

        [JsonProperty("SmallTalksConfig")]
        public Dictionary<string, List<string>> SmallTalksConfig { get; set; }

        [JsonProperty("NameConfig")]
        public NameConfig NameConfig { get; set; }
    }


    public class NameConfig
    {
        [JsonProperty("CleanNameRegexPattern")]
        public string CleanNameRegexPattern { get; set; }
    }

    public class ContentProviderSettings
    {
        [JsonProperty("BaseUrl")]
        public string BaseUrl { get; set; }
        [JsonProperty("confidenceThresholdDefault")]
        public string ConfidenceThresholdDefault { get; set; } = "0.6";
    }

    public class BlipBotSettings
    {
        /// <summary>
        /// BLiP's bot identifier
        /// </summary>
        [JsonProperty("Identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// BLiP's bot access key
        /// </summary>
        [JsonProperty("AccessKey")]
        public string AccessKey { get; set; }
    }
}
