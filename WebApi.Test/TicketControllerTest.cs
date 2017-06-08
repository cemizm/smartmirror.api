using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;
using WebApi.DataLayer.Models;
using Xunit;

namespace WebApi.Test
{
    public class TicketControllerTest
	{
        private Mocks.MirrorRepository mirRep;
		private TicketsController controller;

		public TicketControllerTest()
		{
            this.mirRep = new Mocks.MirrorRepository();
            this.controller = new TicketsController(new Mocks.TicketRepository(), mirRep);
			this.controller.ControllerContext = new ControllerContext()
			{
				HttpContext = new DefaultHttpContext()
			};
		}

		[Fact]
		public async void Should_ReturnTicketNumber_When_KnownMirrorId()
		{
            IActionResult result = await this.controller.Get(Mocks.TicketRepository.TestMirrorId);

			Assert.IsType<OkObjectResult>(result);
			var ok = result as OkObjectResult;

            Assert.IsType<Ticket>(ok.Value);
            Ticket t = ok.Value as Ticket;

            Assert.Equal(Mocks.TicketRepository.TestNumber, t.Number);
		}

		[Fact]
		public async void Should_ReturnNewTicket_When_UnknwonMirrorId()
		{
			IActionResult result = await this.controller.Get(new Guid("{ea8c8ba1-c9cd-4541-8b76-1d11682cde4c}"));

			Assert.IsType<OkObjectResult>(result);
			var ok = result as OkObjectResult;

			Assert.IsType<Ticket>(ok.Value);
			Ticket t = ok.Value as Ticket;

			Assert.Equal(5, t.Number.Length);
		}

		[Fact]
		public async void Should_ReturnBadRequest_When_EmptyMirrorId()
		{
            IActionResult result = await this.controller.Get(Guid.Empty);

            Assert.IsType<BadRequestResult>(result);
		}

		[Fact]
		public async void Should_ReturnBadRequest_When_EmptyTicketNumber()
		{
            IActionResult result = await this.controller.Post(null);

			Assert.IsType<BadRequestResult>(result);
		}

		[Fact]
		public async void Should_ReturnNotFound_When_IncorrectTicketNumber()
		{
			IActionResult result = await this.controller.Post("asdasd");

			Assert.IsType<BadRequestResult>(result);
		}

		[Fact]
		public async void Should_ReturnBadRequest_When_MirrorExists()
		{
			this.controller.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
				new Claim(ClaimTypes.Email, "a@smart.mirror")
			}));

            IActionResult result = await this.controller.Post(Mocks.TicketRepository.TestDupNumber);

            Assert.IsType<BadRequestResult>(result);
		}

		[Fact]
		public async void Should_AddNewMirror_When_CorrectTicketNumber()
		{
			this.controller.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
				new Claim(ClaimTypes.Email, "a@smart.mirror")
			}));

            IActionResult result = await this.controller.Post(Mocks.TicketRepository.TestNumber);

			Assert.IsType<OkObjectResult>(result);
			var ok = result as OkObjectResult;

			Assert.IsType<Mirror>(ok.Value);
            Mirror mirror = ok.Value as Mirror;

            Assert.Equal(Mocks.TicketRepository.TestMirrorId, mirror.Id);
            Assert.Equal("a@smart.mirror", mirror.User);

            Assert.Equal(4, mirRep.GetAll("a@smart.mirror").Result.Count());
		}
    }
}
