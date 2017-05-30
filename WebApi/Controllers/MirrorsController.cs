using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.DataLayer;
using WebApi.DataLayer.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class MirrorsController : Controller
    {
        private IMirrorRepository repository;

        public MirrorsController(IMirrorRepository repository){
            this.repository = repository;
        }

		// GET: api/mirrors
		[HttpGet]
        public async Task<IActionResult> Get()
        {
            var mirrors = await this.repository.GetAll(HttpContext.User.Identity.Name);

            return Ok(mirrors);
        }

        // GET api/mirrors/{936DA01F-9ABD-4D9D-80C7-02AF85C822A8}
		[HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid id)
        {
            Mirror mirror = await this.repository.GetById(id);

            if (mirror == null)
                return NotFound();

            return Ok(mirror);
        }

		// PUT api/mirrors
		[HttpPut]
        public async Task Put([FromBody]Mirror mirror)
        {
            await this.repository.Update(mirror);
        }

		// DELETE api/mirrors/{936DA01F-9ABD-4D9D-80C7-02AF85C822A8}
		[HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await this.repository.Delete(id);
        }
    }
}
