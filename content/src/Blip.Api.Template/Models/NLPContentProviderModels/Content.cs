using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blip.Api.Template.Models.NLPContentProviderModels
{
    public class Content
    {
        [JsonProperty("contentId")]
        public string ContentId { get; set; }

        [JsonProperty("intent")]
        public string Intent { get; set; }

        [JsonProperty("entities")]
        public List<string> Entities { get; set; }

        [JsonProperty("text")]
        public string ContentText { get; set; }

        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }
    }

    public class Tag
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
