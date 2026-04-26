using ASC.Web.Controllers;
using ASC.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Xunit;

namespace ASC.Tests
{
    public class HomeControllerTests
    {
        private readonly IOptions<ApplicationSettings> _options;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            var appSettings = new ApplicationSettings
            {
                ApplicationTitle = "Test Automobile Service Center"
            };
            _options = Options.Create(appSettings);

            _controller = new HomeController(
                NullLogger<HomeController>.Instance,
                _options
            );

            // Thêm HttpContext để tránh lỗi null
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Fact]
        public void HomeController_Index_ReturnsViewResult()
        {
            var result = _controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void HomeController_Index_SetsTitleInViewData()
        {
            var result = _controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var title = viewResult.ViewData["Title"] as string;
            Assert.Equal("Test Automobile Service Center", title);
        }

        [Fact]
        public void HomeController_Privacy_ReturnsViewResult()
        {
            var result = _controller.Privacy();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void HomeController_Error_ReturnsViewResultWithModel()
        {
            var result = _controller.Error();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            Assert.IsType<ErrorViewModel>(viewResult.Model);
        }
    }
}