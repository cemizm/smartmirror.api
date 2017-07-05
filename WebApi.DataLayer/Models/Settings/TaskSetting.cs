using System;
namespace WebApi.DataLayer.Models.Settings
{
    public class TaskSetting : IWidgetSettings
    {
        public int MaxCount { get; set; }
        public string OAuthToken { get; set; }
    }
}
