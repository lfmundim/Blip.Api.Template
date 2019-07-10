using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Blip.Api.Template.Models.NLPContentProviderModels
{
    [DataContract]
    public class ContentResult
    {
     
        [DataMember(Name ="Status")]
        public ContentResultStatus Status { get; set; }

        [DataMember(Name = "Content")]
        public Content Content { get => Contents?.FirstOrDefault(); }

        [DataMember(Name = "Contents")]
        public List<Content> Contents { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
