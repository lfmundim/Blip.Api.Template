using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Blip.Api.Template.Models.NLPContentProviderModels
{
    [DataContract]
    public class ContentAnalysisResult : ContentResult
    {
        [DataMember(Name = "Response")]
        public NLPAnalyseResponse Response { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
