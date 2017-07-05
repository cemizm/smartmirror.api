using System;
namespace WebApi.DataLayer.Models
{
    public enum WidgetType
    {
        Weather = 1,
        Calendar = 2,
        News = 3,
        Mail = 4,
        Task = 5
    }

    public enum WidgetSide
    {
        Inactive = 0,
        Left = 1,
        Right = 2,
    }
}
