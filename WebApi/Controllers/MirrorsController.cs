using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
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
        public IEnumerable<Mirror> Get()
        {
            return this.repository.GetAll("");
        }

		// GET api/mirrors/{936DA01F-9ABD-4D9D-80C7-02AF85C822A8}
		[HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            Mirror mirror = this.repository.GetById("", id);

            if (mirror == null)
                return NotFound();

            return Ok(mirror);
        }

		// PUT api/mirrors
		[HttpPut]
        public void Put([FromBody]Mirror mirror)
        {
            this.repository.Update("", mirror);
        }

		// DELETE api/mirrors/{936DA01F-9ABD-4D9D-80C7-02AF85C822A8}
		[HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            this.repository.Delete("", id);
        }
    }
}
