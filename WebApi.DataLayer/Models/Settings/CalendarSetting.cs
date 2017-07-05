using System;
namespace WebApi.DataLayer.Models.Settings
{
    public class CalendarSetting : IWidgetSettings
	{
		public int MaxCount { get; set; }
		public string OAuthToken { get; set; }
    }
}
