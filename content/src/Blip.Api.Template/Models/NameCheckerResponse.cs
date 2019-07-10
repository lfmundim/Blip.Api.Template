using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Blip.Api.Template.Models
{
    [DataContract]
    public class NameCheckerResponse
    {
        [DataMember(Name="name")]
        public string Input { get; set; }

        [DataMember(Name ="score")]
        public float Score { get; set; }

        [DataMember(Name ="namesFound")]
        public List<string> Matches { get; set; }

    }
}
