using Lime.Protocol;
using System.Globalization;

namespace Blip.Api.Template.Models
{
    public class UserContext : Document
    {
        private static readonly CultureInfo BrazilianCulture = CultureInfo.GetCultureInfo("pt-BR");

        public static readonly MediaType MEDIA_TYPE = MediaType.Parse("application/vnd.lime.apitemplate.usercontext+json");
        public UserContext() : base(MEDIA_TYPE)
        {
            
        }
    }
}
