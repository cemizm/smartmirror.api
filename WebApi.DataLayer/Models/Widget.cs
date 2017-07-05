using System;
using System.Collections.Generic;

namespace WebApi.DataLayer.Models
{
    public class Widget
    {
        public Widget() { }

        /// <summary>
        /// Name of the Widget 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Widget Type
        /// </summary>
        /// <value>The type.</value>
        public WidgetType Type { get; set; }

        /// <summary>
        /// The Side of the Mirror to Display the Widget
        /// </summary>
        public WidgetSide Side { get; set; }

        /// <summary>
        /// The Order Position inside the Mirror Side
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Settings of the Widget
        /// </summary>
        public Dictionary<string, object> Setting { get; set; }
    }
}
