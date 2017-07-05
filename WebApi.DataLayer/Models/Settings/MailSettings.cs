using System;
namespace WebApi.DataLayer.Models.Settings
{
    public class MailSettings : IWidgetSettings
    {
        public int MaxCount { get; set; }
        public bool Unread { get; set; }
        public string OAuthToken { get; set; }
    }
}
