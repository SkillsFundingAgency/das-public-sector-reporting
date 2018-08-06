using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests
{
    [TestFixture]
    public class WhenFormSubmittedWith
    {
        private HomeController _controller;
        private Mock<IUrlHelper> _mockUrlHelper;
        private Mock<IEmployerAccountService> _employeeAccountServiceMock;
        private Mock<IAuthorizationService> _authorizationServiceMock;
        private Mock<IPeriodService> _mockPeriodService;

        [SetUp]
        public void SetUp()
        {
            _mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            _employeeAccountServiceMock = new Mock<IEmployerAccountService>(MockBehavior.Strict);
            _mockPeriodService = new Mock<IPeriodService>();
            _authorizationServiceMock = new Mock<IAuthorizationService>(MockBehavior.Strict);

            _controller = new HomeController(null,_employeeAccountServiceMock.Object, null,_mockPeriodService.Object,_authorizationServiceMock.Object) {Url = _mockUrlHelper.Object};
        }

        [Test]
        public void CreateThenItShouldRedirectToCreateReport()
        {
            // arrange
            var url = "report/create";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

            // act
            var result = _controller.Submit(SubmitActions.Home.Create.SubmitValue);

            // assert
            _mockUrlHelper.VerifyAll();

            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(url, redirectResult.Url);
            Assert.AreEqual(SubmitActions.Home.Create.ActionName, actualContext.Action);
            Assert.AreEqual(SubmitActions.Home.Create.ControllerName, actualContext.Controller);
        }

        [Test]
        public void ListThenItShouldRedirectToReportList()
        {
            // arrange
            var url = "report/list";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

            // act
            var result = _controller.Submit(SubmitActions.Home.List.SubmitValue);

            // assert
            _mockUrlHelper.VerifyAll();

            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(url, redirectResult.Url);
            Assert.AreEqual(SubmitActions.Home.List.ActionName, actualContext.Action);
            Assert.AreEqual(SubmitActions.Home.List.ControllerName, actualContext.Controller);
        }

        [Test]
        public void EditThenItShouldRedirectToReportEdit()
        {
            // arrange
            var url = "report/edit";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

            // act
            var result = _controller.Submit(SubmitActions.Home.Edit.SubmitValue);

            // assert
            _mockUrlHelper.VerifyAll();

            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(url, redirectResult.Url);
            Assert.AreEqual(SubmitActions.Home.Edit.ActionName, actualContext.Action);
            Assert.AreEqual(SubmitActions.Home.Edit.ControllerName, actualContext.Controller);
        }

        [Test]
        public void ViewThenItShouldRedirectToReportEdit()
        {
            // arrange
            var url = "report/summary";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

            // act
            var result = _controller.Submit(SubmitActions.Home.View.SubmitValue);

            // assert
            _mockUrlHelper.VerifyAll();

            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(url, redirectResult.Url);
            Assert.AreEqual(SubmitActions.Home.View.ActionName, actualContext.Action);
            Assert.AreEqual(SubmitActions.Home.View.ControllerName, actualContext.Controller);
        }

        [Test]
        public void UnkownActionThenItShouldReturnError()
        {
            // arrange
            // act
            var result = _controller.Submit("whatever");

            // assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
    }
}
