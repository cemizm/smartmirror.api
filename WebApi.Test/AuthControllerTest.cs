using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApi.Controllers;
using WebApi.DataLayer.Models;
using WebApi.Utils;
using Xunit;

namespace WebApi.Test
{
    public class AuthControllerTest
    {
        private TokenSettings settings;
        private Mocks.UserRepository repository;
        private AuthController controller;

        public AuthControllerTest()
        {
            settings = new TokenSettings();
            settings.Audience = "Test Audience";
            settings.Issuer = "Test Issuer";
            settings.Secret = "SuperSuperTestSecret";

            var options = new OptionsWrapper<TokenSettings>(settings);

            repository = new Mocks.UserRepository();
            this.controller = new AuthController(repository, options);
            this.controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Fact]
        public async void Should_ReturnUnauthorized_OnGet_WhenUserNotAuthorized()
        {

            IActionResult result = await controller.Get();

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async void Should_ReturnUnauthorized_OnGet_WhenUnknownUser()
        {
            this.controller.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Email, "aa@smart.mirror")
            }));

            IActionResult result = await controller.Get();

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async void Should_ReturnOk_OnGet_WhenAllConditionsMet()
        {
            this.controller.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Email, "a@smart.mirror")
            }));

            IActionResult result = await controller.Get();

            Assert.IsType<OkObjectResult>(result);

            OkObjectResult res = result as OkObjectResult;

            Assert.IsType<AuthController.UserBase>(res.Value);

            AuthController.UserBase user = res.Value as AuthController.UserBase;

            Assert.Equal("a@smart.mirror", user.User);
        }

        [Fact]
        public async void Should_ReturnBadRequest_OnLogin_WhenLoginRequestNull()
        {
            IActionResult result = await controller.Login(null);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void Should_ReturnUnauthorized_OnLogin_WhenUnknownUser()
        {
            var login = new AuthController.LoginRequest() { User = "aa@smart.mirror", Password = "asdasd", };

            IActionResult result = await controller.Login(login);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async void Should_ReturnUnauthorized_OnLogin_WhenPasswordNotMatch()
        {
            var login = new AuthController.LoginRequest() { User = Mocks.UserRepository.UserId, Password = "asdasd", };

            IActionResult result = await controller.Login(login);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async void Should_ReturnOk_OnLogin_WhenAllConditionsMet()
        {
            var login = new AuthController.LoginRequest() { User = Mocks.UserRepository.UserId, Password = Mocks.UserRepository.TestPw, };

            IActionResult result = await controller.Login(login);

            Assert.IsType<OkObjectResult>(result);

            OkObjectResult res = result as OkObjectResult;

            Assert.IsType<AuthController.LoginResponse>(res.Value);

            AuthController.LoginResponse user = res.Value as AuthController.LoginResponse;

            Assert.Equal(Mocks.UserRepository.TestName, user.Name);

            TokenManager mngr = new TokenManager(settings);
            var token = mngr.CreateJwtToken(Mocks.UserRepository.TestName, Mocks.UserRepository.UserId);

            Assert.Equal(token, user.AccessToken);
        }

        [Fact]
        public async void Should_ReturnBadRequest_OnRegister_WhenRequestNull()
        {
            IActionResult result = await controller.Register(null);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void Should_ReturnBadRequest_OnRegister_WhenUserAlredyExists()
        {
            var req = new AuthController.RegisterRequest() { Name = "Cem Basoglu", Email = Mocks.UserRepository.UserId, Password = "asdasd", };

            IActionResult result = await controller.Register(req);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void Should_ReturnOk_OnRegister_WhenAllConditionsMet()
        {
            const string testMail = "new@mail.de";

            var req = new AuthController.RegisterRequest() { Name = "Cem Basoglu", Email = testMail, Password = "asdasd", };

            IActionResult result = await controller.Register(req);

            Assert.IsType<OkResult>(result);

            User u = repository.GetByEmail(testMail).Result;

            Assert.NotNull(u);

            Assert.Equal(testMail, u.Email);
        }
    }
}
