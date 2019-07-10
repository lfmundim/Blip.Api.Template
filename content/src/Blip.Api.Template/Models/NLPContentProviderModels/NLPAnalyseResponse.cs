using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Blip.Api.Template.Models.NLPContentProviderModels
{
    [DataContract]
    public class NLPAnalyseResponse
    {
        [DataMember(Name ="Intent")]
        public string Intent { get; set; }
        [DataMember(Name = "IntentScore")]
        public double? IntentScore { get; set; }
        [DataMember(Name = "Entities")]
        public List<string> Entities { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
