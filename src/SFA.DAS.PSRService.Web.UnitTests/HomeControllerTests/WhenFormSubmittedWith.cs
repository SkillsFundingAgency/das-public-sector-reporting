//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using SFA.DAS.PSRService.Web.Controllers;
//using SFA.DAS.PSRService.Web.Services;

//namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests;

//[TestFixture]
//public class WhenFormSubmittedWith
//{
//    private HomeController _controller;
//    private Mock<IUrlHelper> _mockUrlHelper;
//    private Mock<IEmployerAccountService> _employeeAccountServiceMock;
//    private Mock<IAuthorizationService> _authorizationServiceMock;
//    private Mock<IPeriodService> _mockPeriodService;

//    [SetUp]
//    public void SetUp()
//    {
//        _mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
//        _employeeAccountServiceMock = new Mock<IEmployerAccountService>(MockBehavior.Strict);
//        _mockPeriodService = new Mock<IPeriodService>();
//        _authorizationServiceMock = new Mock<IAuthorizationService>(MockBehavior.Strict);

//        _controller = new HomeController(null, _employeeAccountServiceMock.Object, null, _mockPeriodService.Object, _authorizationServiceMock.Object, null, null) { Url = _mockUrlHelper.Object };
//    }

//    [TearDown]
//    public void TearDown() => _controller?.Dispose();

//    [Test]
//    public void CreateThenItShouldRedirectToCreateReport()
//    {
//        // arrange

//        // act
//        var result = _controller.Submit(SubmitActions.Home.Create.SubmitValue);

//        // assert
//        var redirectResult = result as RedirectToActionResult;
//        redirectResult.Should().NotBeNull();
//        redirectResult.ActionName.Should().Be(SubmitActions.Home.Create.ActionName);
//        redirectResult.ControllerName.Should().Be(SubmitActions.Home.Create.ControllerName);
//    }

//    [Test]
//    public void ListThenItShouldRedirectToReportList()
//    {
//        // arrange

//        // act
//        var result = _controller.Submit(SubmitActions.Home.List.SubmitValue);

//        // assert

//        var redirectResult = result as RedirectToActionResult;
//        redirectResult.Should().NotBeNull();
//        redirectResult.ActionName.Should().Be(SubmitActions.Home.List.ActionName);
//        redirectResult.ControllerName.Should().Be(SubmitActions.Home.List.ControllerName);
//    }

//    [Test]
//    public void EditThenItShouldRedirectToReportEdit()
//    {
//        // arrange

//        // act
//        var result = _controller.Submit(SubmitActions.Home.Edit.SubmitValue);

//        // assert
//        var redirectResult = result as RedirectToActionResult;
//        redirectResult.Should().NotBeNull();
//        redirectResult.ActionName.Should().Be(SubmitActions.Home.Edit.ActionName);
//        redirectResult.ControllerName.Should().Be(SubmitActions.Home.Edit.ControllerName);
//    }

//    [Test]
//    public void ViewThenItShouldRedirectToReportEdit()
//    {
//        // arrange

//        // act
//        var result = _controller.Submit(SubmitActions.Home.View.SubmitValue);

//        // assert
//        var redirectResult = result as RedirectToActionResult;
//        redirectResult.Should().NotBeNull();
//        redirectResult.ActionName.Should().Be(SubmitActions.Home.View.ActionName);
//        redirectResult.ControllerName.Should().Be(SubmitActions.Home.View.ControllerName);
//    }

//    [Test]
//    public void UnknownActionThenItShouldReturnError()
//    {
//        // arrange
//        // act
//        var result = _controller.Submit("whatever");

//        // assert
//        result.Should().BeOfType<BadRequestResult>();
//    }
//}