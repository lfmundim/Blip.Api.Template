using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Blip.Api.Template.Models
{

    [DataContract]
    public enum DocumentType
    {
        CPF,
        CNPJ,
        MercosurVehiclePlate,
        BrazilianVehiclePlate,
        None
    }
}
