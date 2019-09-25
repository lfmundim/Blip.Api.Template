namespace Blip.Api.Template.Models.Ui
{
    /// <summary>
    /// Class to use data from appsettings.json "Settings" field
    /// </summary>
    public class ApiSettings
    {
        /// <summary>
        /// Current API Version
        /// </summary>
        public string ApiVersion { get; set; }

        /// <summary>
        /// BLiP's bot identifier and access key
        /// </summary>
        public BlipBotSettings BlipBotSettings { get; set; }
    }

    public class BlipBotSettings
    {
        /// <summary>
        /// BLiP's bot identifier
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// BLiP's bot access key
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// BLiP's bot Authorization Key
        /// </summary>
        public string Authorization { get; set; }
    }
}
