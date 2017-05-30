using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DataLayer;
using WebApi.DataLayer.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TicketsController : BaseController
    {
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

            mirror = new Mirror() { Id = ticket.MirrorId, Name = "Smart Mirror", User = UserEmail };

            await mirRepository.Add(mirror);

            return Ok(mirror);
        }
    }
}
