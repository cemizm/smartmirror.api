using System;
namespace WebApi.DataLayer.Models
{
    public class Ticket
    {
        public Ticket() { }

        /// <summary>
        /// The Ticket number used to register a mirror
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// The Id of the Mirror which has requested the ticket
        /// </summary>
        public Guid MirrorId { get; set; }
    }
}
