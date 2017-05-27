using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;
using WebApi.DataLayer;
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
        }

        [Fact]
        public void Should_Return3Items_When_UserA()
        {
            IEnumerable<Mirror> mirrors = this.controller.Get();

            Assert.Equal(3, mirrors.Count());
		}

		[Fact]
		public void Should_Return1Item_When_UserB()
		{
			IEnumerable<Mirror> mirrors = this.controller.Get();

			Assert.Equal(1, mirrors.Count());
		}

		[Fact]
		public void Should_ReturnNoItem_When_UserUnknown()
		{
			IEnumerable<Mirror> mirrors = this.controller.Get();

			Assert.Equal(0, mirrors.Count());
		}

		[Fact]
		public void Should_ReturnCorrectMirror_WhenRequestedById()
		{
			IActionResult result = this.controller.Get(Mocks.MirrorRepository.TestId);

			var ok = result as OkObjectResult;
			Mirror m = ok.Value as Mirror;

			Assert.IsType<OkObjectResult>(result);
			Assert.IsType<Mirror>(ok.Value);
			Assert.Equal(Mocks.MirrorRepository.TestId, m.Id);
		}

		[Fact]
		public void Should_ReturnNotFound_WhenEmptyMirrorId()
		{
            IActionResult result = this.controller.Get(Guid.Empty);

            Assert.IsType<NotFoundResult>(result);
		}
    }
}
