using System;
namespace WebApi.DataLayer.Models.Settings
{
    public class NewsSetting : IWidgetSettings
	{
		public string FeedUrl { get; set; }
		public int MaxCount { get; set; }
    }
}
