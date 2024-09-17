using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.UnitTests.QuestionControllerTests;

[TestFixture]
public class Given_I_Request_A_Question
{
    private QuestionController _controller;
    private Mock<IReportService> _reportService;
    private Mock<IEmployerAccountService> _employerAccountServiceMock;
    private Mock<IUrlHelper> _mockUrlHelper;
    private Mock<IUserService> _mockUserService;
    private Mock<IPeriodService> _periodServiceMock;
    private EmployerIdentifier _employerIdentifier;

    [SetUp]
    public void SetUp()
    {
        _mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
        _employerAccountServiceMock = new Mock<IEmployerAccountService>(MockBehavior.Strict);
        _reportService = new Mock<IReportService>(MockBehavior.Strict);

        _periodServiceMock = new Mock<IPeriodService>(MockBehavior.Strict);
        _periodServiceMock.Setup(s => s.GetCurrentPeriod()).Returns(Period.FromInstantInPeriod(DateTime.UtcNow));

        _mockUserService = new Mock<IUserService>(MockBehavior.Strict);

        _controller = new QuestionController(
                _reportService.Object,
                _employerAccountServiceMock.Object,
                null,
                _periodServiceMock.Object,
                _mockUserService.Object)
            { Url = _mockUrlHelper.Object };

        _employerIdentifier = new EmployerIdentifier { AccountId = "ABCDE", EmployerName = "EmployerName" };

        _employerAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(It.IsAny<HttpContext>())).Returns(_employerIdentifier);
        _employerAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(null)).Returns(_employerIdentifier);
    }

    [TearDown]
    public void TearDown() => _controller?.Dispose();

    [Test]
    public async Task And_A_Report_Does_Not_Exist_Then_Redirect_Home()
    {
        // arrange
        const string url = "home/index";
        UrlActionContext actualContext = null;

        _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");
        _reportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((Report)null);
        _reportService.Setup(s => s.CanBeEdited(null)).Returns(false).Verifiable();

        // act
        var result = await _controller.Index("YourEmployees");

        // assert
        _mockUrlHelper.VerifyAll();

        var redirectResult = result as RedirectResult;
        redirectResult.Should().NotBeNull();
        redirectResult.Url.Should().Be(url);
        actualContext.Action.Should().Be("Index");
        actualContext.Controller.Should().Be("Home");
    }

    [Test]
    public async Task And_A_Valid_Report_Does_Not_Exist_Then_Redirect_Home()
    {
        // arrange
        var url = "home/index";
        UrlActionContext actualContext = null;

        var report = new ReportBuilder()
            .WithInvalidSections()
            .WithEmployerId("ABCDE")
            .ForCurrentPeriod()
            .WhereReportIsNotAlreadySubmitted()
            .Build();

        _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");
        _reportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(report);
        _reportService.Setup(s => s.CanBeEdited(report)).Returns(false).Verifiable();

        // act
        var result = await _controller.Index("YourEmployees");

        // assert
        _mockUrlHelper.VerifyAll();

        var redirectResult = result as RedirectResult;
        redirectResult.Should().NotBeNull();
        redirectResult.Url.Should().Be(url);
        actualContext.Action.Should().Be("Index");
        actualContext.Controller.Should().Be("Home");
    }

    [Test]
    public async Task And_The_Question_ID_Does_Not_Exist_Then_Return_Error()
    {
        // arrange
        var url = "home/index";
        UrlActionContext actualContext = null;

        _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");
        _reportService.Setup(s => s.CanBeEdited(It.IsAny<Report>())).Returns(true).Verifiable();

        var stubReport = new ReportBuilder()
            .WithValidSections()
            .WithEmployerId("ABCDE")
            .WhereReportIsNotAlreadySubmitted()
            .ForCurrentPeriod()
            .Build();

        _reportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(stubReport);

        // act
        var result = await _controller.Index("YourEmployees");

        // assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task The_Question_ID_Exists_And_Report_Is_Valid_Then_Show_Question_Page()
    {
        const string url = "home/index";
        UrlActionContext actualContext = null;

        _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

        var stubReport = new ReportBuilder()
            .WithValidSections()
            .WithEmployerId("ABCDE")
            .ForCurrentPeriod()
            .WhereReportIsNotAlreadySubmitted()
            .Build();

        _reportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(stubReport);
        _reportService.Setup(s => s.CanBeEdited(It.IsAny<Report>())).Returns(true).Verifiable();

        // act
        var result = await _controller.Index("SectionOne");

        // assert
        var listViewResult = result as ViewResult;
        listViewResult.Should().NotBeNull();
        listViewResult.ViewName.Should().Be("Index", "View name does not match, should be: Index");

        var sectionViewModel = listViewResult.Model as SectionViewModel;
        sectionViewModel.Should().NotBeNull();

        var report = sectionViewModel.Report;
        report.Should().NotBeNull();

        var questionSection = sectionViewModel.CurrentSection;
        questionSection.Should().NotBeNull();
        questionSection.Id.Should().Be("SectionOne");

        var sectionOneQuestions = stubReport.Sections.Single(s => s.Id == "SectionOne").Questions;

        questionSection.Questions.Should().BeEquivalentTo(sectionOneQuestions);
    }
}