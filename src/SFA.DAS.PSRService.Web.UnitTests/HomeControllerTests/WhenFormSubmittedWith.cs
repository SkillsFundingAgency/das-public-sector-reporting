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

            _controller = new HomeController(null,_employeeAccountServiceMock.Object, null,_mockPeriodService.Object,_authorizationServiceMock.Object, null, null) {Url = _mockUrlHelper.Object};
        }

        [Test]
        public void CreateThenItShouldRedirectToCreateReport()
        {
            // arrange

            // act
            var result = _controller.Submit(SubmitActions.Home.Create.SubmitValue);

            // assert
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(SubmitActions.Home.Create.ActionName, redirectResult.ActionName);
            Assert.AreEqual(SubmitActions.Home.Create.ControllerName, redirectResult.ControllerName);
        }

        [Test]
        public void ListThenItShouldRedirectToReportList()
        {
            // arrange

            // act
            var result = _controller.Submit(SubmitActions.Home.List.SubmitValue);

            // assert

            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(SubmitActions.Home.List.ActionName, redirectResult.ActionName);
            Assert.AreEqual(SubmitActions.Home.List.ControllerName, redirectResult.ControllerName);
        }

        [Test]
        public void EditThenItShouldRedirectToReportEdit()
        {
            // arrange

            // act
            var result = _controller.Submit(SubmitActions.Home.Edit.SubmitValue);

            // assert
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(SubmitActions.Home.Edit.ActionName, redirectResult.ActionName);
            Assert.AreEqual(SubmitActions.Home.Edit.ControllerName, redirectResult.ControllerName);
        }

        [Test]
        public void ViewThenItShouldRedirectToReportEdit()
        {
            // arrange

            // act
            var result = _controller.Submit(SubmitActions.Home.View.SubmitValue);

            // assert
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(SubmitActions.Home.View.ActionName, redirectResult.ActionName);
            Assert.AreEqual(SubmitActions.Home.View.ControllerName, redirectResult.ControllerName);
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
