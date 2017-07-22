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
            if (UserEmail == null)
                return Unauthorized();

            var user = await repository.GetByEmail(UserEmail);
            if (user == null)
                return Unauthorized();

            return Ok(new UserBase(user));
        }

        // POST api/auth
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginRequest req)
        {
            if (req == null)
				return BadRequest();

			if (!ModelState.IsValid)
				return BadRequest();

            var user = await repository.GetByEmailPassword(req.User, req.Password);
            if (user == null)
                return Unauthorized();

            string token = manager.CreateJwtToken(user.Name, user.Email);

            LoginResponse response = new LoginResponse(user)
            {
                AccessToken = token,
            };

            return Ok(response);
		}

		// POST api/auth
		[HttpPost("register")]
		[AllowAnonymous]
		public async Task<IActionResult> Register([FromBody]RegisterRequest req)
		{
			if (req == null)
				return BadRequest();

			if (!ModelState.IsValid)
				return BadRequest();

			var user = await repository.GetByEmail(req.Email);
			if (user != null)
				return BadRequest();

			user = new DataLayer.Models.User();
			user.Email = req.Email;
			user.Name = req.Name;
			user.Password = req.Password;

			await repository.Add(user);

			return Ok();
		}

		// POST api/auth
		[HttpPost("token")]
		[AllowAnonymous]
        public async Task<IActionResult> Token([FromForm]TokenRequest req)
		{
			if (req == null)
				return BadRequest();

			if (!ModelState.IsValid)
				return BadRequest();

            var token = manager.GetToken(req.code);
            var email = token.Claims.Where(c => c.Type == "email").Select(c => c.Value).FirstOrDefault();

			if (email == null)
				return Unauthorized();

			var user = await repository.GetByEmail(email);
			if (user == null)
				return Unauthorized();
            
            string jwt = manager.CreateJwtToken(user.Name, user.Email,DateTime.Now.AddDays(180));

            var res = new AccessTokenResponse()
            {
                access_token = jwt,
                expires_in = 60 * 60 * 24 * 180,
                refresh_token = null,
                token_type = "bearer"
            };


			return Ok(res);
		}

        #region Nested Types

        public class UserBase
        {
            public UserBase(DataLayer.Models.User user)
            {
                this.User = user.Email;
                this.Name = user.Name;
            }
            /// <summary>
            /// Email of User
            /// </summary>
            public string User { get; private set; }

            /// <summary>
            /// Name of User
            /// </summary>
            public string Name { get; private set; }

        }

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

        public class RegisterRequest
        {
            [Required]
            public string Email { get; set; }

            [Required]
            public string Name { get; set; }

            [Required]
            public string Password { get; set; }
        }

        public class LoginResponse : UserBase
        {
            public LoginResponse(DataLayer.Models.User user) : base(user)
            {

            }

            /// <summary>
            /// User AccessToken 
            /// </summary>
            public string AccessToken { get; set; }

        }

        public class TokenRequest {
            [Required]
            public string code { get; set; }
        }

		public class AccessTokenResponse
		{
			public string access_token { get; set; }
			public string token_type { get; set; }
			public int expires_in { get; set; }
            public string refresh_token { get; set; }
        }

        #endregion
    }
}
