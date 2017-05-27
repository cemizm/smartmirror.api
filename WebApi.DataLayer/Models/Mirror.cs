using System;
using System.Collections.Generic;

namespace WebApi.DataLayer.Models
{
    public class Mirror
    {
        public Mirror() { }

        /// <summary>
        /// Id of the Mirror
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The Name of the Mirror used to display on administration operations.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Path to the Image used to display on administration operations.
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// The Widgets to Display in the Mirror frontend
        /// </summary>
        public List<Widget> Widgets { get; set; }
    }
}
