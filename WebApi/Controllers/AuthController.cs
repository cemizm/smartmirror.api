using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApi.DataLayer;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private IUserRepository repository;

        private Utils.TokenManager manager;

        public AuthController(IUserRepository repository, IOptions<Utils.TokenSettings> settings)
        {
            this.repository = repository;

            this.manager = new Utils.TokenManager(settings.Value);
        }

        // GET: api/auth
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await repository.GetByEmail(UserEmail);

            if (user == null)
                return Unauthorized();

            return Ok(user);
        }

        // POST api/auth
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody]LoginRequest req)
        {
            if (req == null)
                return BadRequest();

            var user = await repository.GetByEmailPassword(req.User, req.Password);
            if (user == null)
                return Unauthorized();

            string token = manager.CreateJwtToken(user.Name, user.Email);

            LoginResponse response = new LoginResponse(){
                AccessToken = token,
                User = user.Email,
                Name = user.Name
            };

            return Ok(response);
        }

        #region Nested Types

        public class LoginRequest
        {
            /// <summary>
            /// Email of User
            /// </summary>
            [Required]
            public string User { get; set; }

            /// <summary>
            /// MD5 Password
            /// </summary>
            [Required]
            public string Password { get; set; }
        }

        public class LoginResponse
        {
            /// <summary>
            /// Email of User
            /// </summary>
            public string User { get; set; }

            /// <summary>
            /// Name of User
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// User AccessToken 
            /// </summary>
            public string AccessToken { get; set; }

        }

        #endregion
    }
}
