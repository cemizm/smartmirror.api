using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.DataLayer;
using WebApi.DataLayer.Models;
using WebApi.Utils;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class MirrorsController : BaseController
    {
        private IMirrorRepository repository;
        private SocketPublisher socket;

        public MirrorsController(IMirrorRepository repository, SocketPublisher socket)
        {
            this.repository = repository;
            this.socket = socket;
        }

        // GET: api/mirrors
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (string.IsNullOrEmpty(UserEmail))
                return Unauthorized();

            var mirrors = await this.repository.GetAll(UserEmail);

            return Ok(mirrors);
        }

        // GET api/mirrors/{936DA01F-9ABD-4D9D-80C7-02AF85C822A8}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();

            Mirror mirror = await this.repository.GetById(id);
            if (mirror == null)
                return NotFound();

            return Ok(mirror);
        }

        // PUT api/mirrors
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]Mirror mirror)
        {
            if (mirror.Id == Guid.Empty)
                return BadRequest();

            Mirror existing = await this.repository.GetById(mirror.Id);
            if (mirror == null)
                return NotFound();

            if (existing.User != UserEmail.ToLower())
                return Unauthorized();

            mirror.User = UserEmail;

            await this.repository.Update(mirror);

			this.socket.UpdateMirror(mirror);

            return Ok();
        }

        // DELETE api/mirrors/{936DA01F-9ABD-4D9D-80C7-02AF85C822A8}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Mirror mirror = await this.repository.GetById(id);
            if (mirror == null)
                return NotFound();

            if (mirror.User != UserEmail.ToLower())
                return Unauthorized();

            await this.repository.Delete(id);

            return Ok();
        }
    }
}
