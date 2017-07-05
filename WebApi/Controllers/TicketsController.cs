using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DataLayer;
using WebApi.DataLayer.Models;
using WebApi.DataLayer.Models.Settings;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TicketsController : BaseController
    {
        private static string[] images = new string[] { "https://www.galileo.tv/app/uploads/2016/02/Smart-Mirror-zum-selber-bauen.jpg", 
                                                        "https://i1.wp.com/www.spiegelschrank-kaufen.de/wp-content/uploads/2016/06/Microsoft-Magic-Mirror-Mockup.jpg?w=595&ssl=1https://spiegelshop24.com/images/3.jpg",
                                                        "http://images.ifun.de/wp-content/uploads/2015/10/spiegel.jpg", 
                                                        "https://www.klonblog.com/images/2013/10/klonblog-004-smartmirror.jpg",
                                                        "https://glancr.de/wp-content/uploads/glancr-freisteller.png",
                                                        "https://s-media-cache-ak0.pinimg.com/736x/84/b4/86/84b486aa84ac6c49f90cf54df1a742eb.jpg"
                                                      };

        private ITicketRepository repository;
        private IMirrorRepository mirRepository;

        public TicketsController(ITicketRepository repository, IMirrorRepository mirRepository)
        {
            this.repository = repository;
            this.mirRepository = mirRepository;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid id)
        {
            if (Guid.Empty == id)
                return BadRequest();

            var ticket = await repository.GetTicketByMirrorId(id);
            if (ticket == null)
            {
                ticket = new Ticket() { MirrorId = id, Number = Utils.TicketGenerator.Generate() };
                await repository.Add(ticket);
            }

            return Ok(ticket);
        }

        [HttpPost("{number}")]
        public async Task<IActionResult> Post([FromRoute]string number)
        {
            if (string.IsNullOrEmpty(number))
                return BadRequest();

            var ticket = await repository.GetTicket(number);
            if (ticket == null)
                return BadRequest();

            var mirror = await mirRepository.GetById(ticket.MirrorId);
            if (mirror != null) //already exists
                return BadRequest();

            mirror = new Mirror()
            {
                Id = ticket.MirrorId,
                Name = "Smart Mirror",
                User = UserEmail,
                Image = images[new Random().Next(0, images.Length - 1)],
                Widgets = new List<Widget>(new Widget[]{
                    new Widget() { Name = "News", Order = 0, Side= WidgetSide.Left, Type= WidgetType.News, Setting = new NewsSetting() { MaxCount=3, FeedUrl=""}},
                    new Widget() { Name = "Mail", Order = 1, Side= WidgetSide.Left, Type= WidgetType.Mail, Setting = new MailSettings() { MaxCount=3 }},
                    new Widget() { Name = "Wetter", Order = 0, Side= WidgetSide.Right, Type= WidgetType.Weather, Setting = new WeatherSetting() { City="Bielefeld"}},
                    new Widget() { Name = "Task", Order = 1, Side= WidgetSide.Right, Type= WidgetType.Task, Setting = new TaskSetting() { MaxCount=3 }},
                    new Widget() { Name = "Calendar", Order = 2, Side= WidgetSide.Right, Type= WidgetType.Calendar, Setting = new CalendarSetting() { MaxCount=3}}
                })
            };

            await mirRepository.Add(mirror);

            return Ok(mirror);
        }
    }
}
