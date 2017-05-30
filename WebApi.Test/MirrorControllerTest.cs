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

        public MirrorControllerTest()
        {

            this.controller = new MirrorsController(new Mocks.MirrorRepository());
            this.controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Fact]
        public async void Should_Return3Items_When_UserA()
        {
            this.controller.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Name, "a@smart.mirror")
            }));

            IActionResult result = await this.controller.Get();

            Assert.IsType<OkObjectResult>(result);

            var ok = result as OkObjectResult;
            Assert.IsType<List<Mirror>>(ok.Value);
            List<Mirror> mirrors = ok.Value as List<Mirror>;

            Assert.Equal(3, mirrors.Count());
        }

        [Fact]
        public async void Should_Return1Item_When_UserB()
		{
			this.controller.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
				new Claim(ClaimTypes.Name, "b@smart.mirror")
			}));

            IActionResult result = await this.controller.Get();

            Assert.IsType<OkObjectResult>(result);

            var ok = result as OkObjectResult;
            Assert.IsType<List<Mirror>>(ok.Value);
            List<Mirror> mirrors = ok.Value as List<Mirror>;

            Assert.Equal(1, mirrors.Count());
        }

        [Fact]
        public async void Should_Return0Items_When_UserUnknown()
		{

			IActionResult result = await this.controller.Get();

			Assert.IsType<OkObjectResult>(result);

			var ok = result as OkObjectResult;
			Assert.IsType<List<Mirror>>(ok.Value);
			List<Mirror> mirrors = ok.Value as List<Mirror>;

			Assert.Equal(0, mirrors.Count());
        }

        [Fact]
        public async void Should_ReturnCorrectMirror_WhenRequestedById()
        {
            IActionResult result = await this.controller.Get(Mocks.MirrorRepository.TestId);

            var ok = result as OkObjectResult;
            Mirror m = ok.Value as Mirror;

            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<Mirror>(ok.Value);
            Assert.Equal(Mocks.MirrorRepository.TestId, m.Id);
        }

        [Fact]
        public async void Should_ReturnNotFound_WhenEmptyMirrorId()
        {
            IActionResult result = await this.controller.Get(Guid.Empty);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
