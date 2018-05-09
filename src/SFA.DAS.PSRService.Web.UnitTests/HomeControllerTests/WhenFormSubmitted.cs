using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests
{
    [TestFixture]
    public class WhenFormSubmitted
    {
        private HomeController _controller;
        private Mock<IUrlHelper> _mockUrlHelper;
        private Mock<IEmployerAccountService> _employeeAccountServiceMock;

        private Mock<IPeriodService> _mockPeriodService;

        [SetUp]
        public void SetUp()
        {
            _mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            _employeeAccountServiceMock = new Mock<IEmployerAccountService>(MockBehavior.Strict);
            _mockPeriodService = new Mock<IPeriodService>();
            _controller = new HomeController(null,_employeeAccountServiceMock.Object, null,_mockPeriodService.Object) {Url = _mockUrlHelper.Object};
        }

        [Test]
        public void ThenItShouldRedirectToCreateReport()
        {
            // arrange
            var url = "report/create";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

            // act
            var result = _controller.Submit("create");

            // assert
            _mockUrlHelper.VerifyAll();

            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(url, redirectResult.Url);
            Assert.AreEqual("Create", actualContext.Action);
            Assert.AreEqual("Report", actualContext.Controller);
        }

        [Test]
        public void ThenItShouldRedirectToReportList()
        {
            // arrange
            var url = "report/list";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

            // act
            var result = _controller.Submit("list");

            // assert
            _mockUrlHelper.VerifyAll();

            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(url, redirectResult.Url);
            Assert.AreEqual("List", actualContext.Action);
            Assert.AreEqual("Report", actualContext.Controller);
        }

        [Test]
        public void ThenItShouldReturnErrorIfUnknownActionPassed()
        {
            // arrange
            // act
            var result = _controller.Submit("whatever");

            // assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
    }
}
