using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Blip.Api.Template.Models
{

    [DataContract]
    public class ValidateBaseResponse : GenericBaseResponse 
    {
        [DataMember(Name ="isValid")]
        public bool IsValid { get; set; } = false;
    }
}
