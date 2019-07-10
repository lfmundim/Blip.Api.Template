using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Blip.Api.Template.Models
{
    [DataContract]
    public class ValidateDocumentResponse : ValidateBaseResponse
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [DataMember(Name="type")]
        public DocumentType Type { get; set; }
    }

}
