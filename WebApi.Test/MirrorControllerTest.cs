using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;
using WebApi.DataLayer.Models;
using Xunit;

namespace WebApi.Test
{
    public class MirrorControllerTest
    {
        private MirrorsController controller;
        private Mocks.MirrorRepository repository;

        public MirrorControllerTest()
        {
            repository = new Mocks.MirrorRepository();
            this.controller = new MirrorsController(repository, new DummySocket());
            this.controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Fact]
        public async void Should_Return3Items_OnGet_WhenUserA()
        {
            this.controller.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Email, "a@smart.mirror")
            }));

            IActionResult result = await this.controller.Get();

            Assert.IsType<OkObjectResult>(result);

            var ok = result as OkObjectResult;
            Assert.IsType<List<Mirror>>(ok.Value);
            List<Mirror> mirrors = ok.Value as List<Mirror>;

            Assert.Equal(3, mirrors.Count());
        }

        [Fact]
        public async void Should_Return1Item_OnGet_WhenUserB()
		{
			this.controller.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Email, "b@smart.mirror")
			}));

            IActionResult result = await this.controller.Get();

            Assert.IsType<OkObjectResult>(result);

            var ok = result as OkObjectResult;
            Assert.IsType<List<Mirror>>(ok.Value);
            List<Mirror> mirrors = ok.Value as List<Mirror>;

            Assert.Equal(1, mirrors.Count());
        }

        [Fact]
        public async void Should_ReturnUnauthorized_OnGet_WhenUserUnknown()
		{
			IActionResult result = await this.controller.Get();

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async void Should_ReturnCorrectMirror_OnGet_WhenRequestedById()
        {
            IActionResult result = await this.controller.Get(Mocks.MirrorRepository.TestId);

            var ok = result as OkObjectResult;
            Mirror m = ok.Value as Mirror;

            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<Mirror>(ok.Value);
            Assert.Equal(Mocks.MirrorRepository.TestId, m.Id);
		}

		[Fact]
		public async void Should_ReturnBadRequest_OnGet_WhenEmptyMirrorId()
		{
			IActionResult result = await this.controller.Get(Guid.Empty);

			Assert.IsType<BadRequestResult>(result);
		}

		[Fact]
		public async void Should_ReturnNotFound_OnGet_WhenUnknownMirrorId()
		{
			IActionResult result = await this.controller.Get(Guid.NewGuid());

			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async void Should_ReturnOK_OnUpdate_WhenAllConditionsMet()
		{
			var mirror = repository.GetById(Mocks.MirrorRepository.TestId).Result;
            var copy = new Mirror()
            {
                Id = mirror.Id,
                Name = "Updated Text",
                User = mirror.User,
                Image = mirror.Image,
                Widgets = mirror.Widgets,
            };

			this.controller.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
				new Claim(ClaimTypes.Email, "a@smart.mirror")
			}));

			IActionResult result = await this.controller.Put(copy);

            mirror = repository.GetById(Mocks.MirrorRepository.TestId).Result;

            Assert.IsType<OkResult>(result);
            Assert.Equal(copy.Name, mirror.Name);
		}

		[Fact]
		public async void Should_ReturnUnauthorized_OnUpdate_WhenNotOwnMirror()
		{
			var mirror = repository.GetById(Mocks.MirrorRepository.TestId).Result;

			this.controller.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
				new Claim(ClaimTypes.Email, "b@smart.mirror")
			}));

			IActionResult result = await this.controller.Put(mirror);

			Assert.IsType<UnauthorizedResult>(result);
		}

		[Fact]
		public async void Should_ReturnBadRequest_OnUpdate_WhenMirrorIdEmpty()
		{
            var mirror = new Mirror() { Id = Guid.Empty };

			this.controller.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
				new Claim(ClaimTypes.Email, "b@smart.mirror")
			}));

			IActionResult result = await this.controller.Put(mirror);

            Assert.IsType<BadRequestResult>(result);
		}

		[Fact]
		public async void Should_ReturnNotFound_OnDelete_WhenMirrorIdEmpty()
		{
			this.controller.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
				new Claim(ClaimTypes.Email, "b@smart.mirror")
			}));

			IActionResult result = await this.controller.Delete(Guid.Empty);

			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async void Should_ReturnBadRequest_OnDelete_WhenNotOwnMirror()
		{
			this.controller.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
				new Claim(ClaimTypes.Email, "b@smart.mirror")
			}));

            IActionResult result = await this.controller.Delete(Mocks.MirrorRepository.TestId);

            Assert.IsType<UnauthorizedResult>(result);
		}

		[Fact]
		public async void Should_ReturnOk_OnDelete_WhenAllConditionMet()
		{
			this.controller.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
				new Claim(ClaimTypes.Email, "a@smart.mirror")
			}));

			IActionResult result = await this.controller.Delete(Mocks.MirrorRepository.TestId);

            Assert.IsType<OkResult>(result);
		}
    }
}
