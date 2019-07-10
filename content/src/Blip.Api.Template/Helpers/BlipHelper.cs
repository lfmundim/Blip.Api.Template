using Lime.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blip.Api.Template.Helpers
{
    public class BlipHelper
    {
        public static string GetBotAuthorization(string identifier, string accessKey)
        {
            return $"{identifier}:{accessKey.FromBase64()}".ToBase64();
        }

        public static string ExtractIdentityName(string identity)
        {
            var result = Identity.TryParse(identity, out Identity value);
            return result ? value.Name : identity;
        }

        public static string ExtractIdentityDomain(string identity)
        {
            var result = Identity.TryParse(identity, out Identity value);
            return result ? value.Domain : identity;
        }

        public static ChannelInfo GetChannelInfo(string identity)
        {
            var result = ExtractIdentityDomain(identity);
            switch (result)
            {
                case "businesschat.gw.msging.net":
                    return ChannelInfo.AppleBusinessChat;
                default:
                    return ChannelInfo.Unkown;
            }
        }

        public enum ChannelInfo
        {
            AppleBusinessChat,
            Messenger,
            Whatsapp,
            Unkown

        }
    }
}
