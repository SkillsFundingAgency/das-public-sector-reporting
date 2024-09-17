using System.Linq;
using System.Security.Claims;
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
public class Given_I_Submit_A_Question
{
    private QuestionController _controller;
    private Mock<IReportService> _reportService;
    private Mock<IEmployerAccountService> _employerAccountServiceMock;
    private Mock<IUrlHelper> _mockUrlHelper;
    private Mock<IUserService> _mockUserService;

    private EmployerIdentifier _employerIdentifier;
    private Mock<IPeriodService> _periodServiceMock;
    private Report _currentValidAndNotSubmittedReport;

    [SetUp]
    public void SetUp()
    {
        _mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
        _employerAccountServiceMock = new Mock<IEmployerAccountService>(MockBehavior.Strict);
        _reportService = new Mock<IReportService>(MockBehavior.Strict);
        _periodServiceMock = new Mock<IPeriodService>(MockBehavior.Strict);
        _mockUserService = new Mock<IUserService>(MockBehavior.Strict);


        _employerIdentifier = new EmployerIdentifier { AccountId = "ABCDE", EmployerName = "EmployerName" };

        _employerAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(It.IsAny<HttpContext>()))
            .Returns(_employerIdentifier);
        _employerAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(null))
            .Returns(_employerIdentifier);

        _controller = new QuestionController(_reportService.Object, _employerAccountServiceMock.Object, null, _periodServiceMock.Object, _mockUserService.Object) { Url = _mockUrlHelper.Object };

        _currentValidAndNotSubmittedReport =
            new ReportBuilder()
                .WithValidSections()
                .WithEmployerId("ABCDEF")
                .ForCurrentPeriod()
                .WhereReportIsNotAlreadySubmitted()
                .Build();
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
        _reportService.Setup(s => s.GetReport("111", It.IsAny<string>())).ReturnsAsync((Report)null).Verifiable();
        _reportService.Setup(s => s.CanBeEdited(null)).Returns(false).Verifiable();

        // act
        var result = await _controller.Submit(new SectionModel { ReportingPeriod = "111" });

        // assert
        _mockUrlHelper.VerifyAll();
        _reportService.VerifyAll();

        var redirectResult = result as RedirectResult;
        redirectResult.Should().NotBeNull();
        redirectResult.Url.Should().Be(url);
        actualContext.Action.Should().Be("Index");
        actualContext.Controller.Should().Be("Home");
    }

    [Test]
    public async Task And_Report_Is_Not_Editable_Then_Redirect_Home()
    {
        // arrange
        const string url = "home/index";
        UrlActionContext actualContext = null;
        var report = new Report();

        _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

        _reportService.Setup(s => s.GetReport("111", It.IsAny<string>())).ReturnsAsync(report).Verifiable();
        _reportService.Setup(s => s.CanBeEdited(report)).Returns(false).Verifiable();

        // act
        var result = await _controller.Submit(new SectionModel { ReportingPeriod = "111" });

        // assert
        _mockUrlHelper.VerifyAll();
        _reportService.VerifyAll();

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
        _reportService.Setup(s => s.GetReport("111", It.IsAny<string>())).ReturnsAsync(_currentValidAndNotSubmittedReport).Verifiable();
        _reportService.Setup(s => s.CanBeEdited(It.IsAny<Report>())).Returns(true).Verifiable();

        // act
        var result = await _controller.Submit(new SectionModel { ReportingPeriod = "111", Id = "No such section" });

        // assert
        _reportService.VerifyAll();
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task The_SectionViewModel_Is_Valid_Then_Save_Question_Section()
    {
        Report actualReport = null;
        var url = "home/index";
        UrlActionContext actualContext = null;

        _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

        _reportService.Setup(s => s.GetReport("222", It.IsAny<string>())).ReturnsAsync(_currentValidAndNotSubmittedReport).Verifiable();
        _reportService.Setup(s => s.SaveReport(It.IsAny<Report>(), It.IsAny<UserModel>(), null)).Callback<Report, UserModel, bool?>((r, u, s) => actualReport = r).Returns(() => Task.CompletedTask).Verifiable("Report was not saved");
        _reportService.Setup(s => s.CanBeEdited(It.IsAny<Report>())).Returns(true);
        _mockUserService.Setup(s => s.GetUserModel(It.IsAny<ClaimsPrincipal>())).Returns(new UserModel()).Verifiable();

        var sectionModel = new SectionModel
        {
            Id = "SubSectionOne",
            ReportingPeriod = "222",
            Questions =
            [
                new QuestionViewModel
                {
                    Id = "atEnd",
                    Answer = "123,000"
                },
                new QuestionViewModel
                {
                    Id = "atStart",
                    Answer = "123"
                }
            ]
        };

        // act
        var result = await _controller.Submit(sectionModel);

        // assert
        _reportService.VerifyAll();
        _mockUserService.VerifyAll();

        var redirectResult = result as RedirectResult;
        redirectResult.Should().NotBeNull();
        redirectResult.Url.Should().Be(url);
        actualContext.Action.Should().Be("Edit");
        actualContext.Controller.Should().Be("Report");

        actualReport.Should().NotBeNull();

        var section = actualReport.GetQuestionSection("SubSectionOne");

        section.Should().NotBeNull();
        section.Questions.Count().Should().Be(3);
        section.Questions.Single(q => q.Id == "atStart").Answer.Should().Be("123");
        section.Questions.Single(q => q.Id == "atEnd").Answer.Should().Be("123000");

        var originalNewThisPeriodAnswer = _currentValidAndNotSubmittedReport
            .GetQuestionSection("SubSectionOne")
            .Questions
            .Single(q => q.Id == "newThisPeriod")
            .Answer;

        section.Questions.Single(q => q.Id == "newThisPeriod").Answer.Should().Be(originalNewThisPeriodAnswer);
    }

    [Test]
    public async Task The_SectionViewModel_Is_Valid_But_Report_Is_Not_Full_Then_Save_Question_Section()
    {
        // arrange
        Report actualReport = null;
        
        var report = new ReportBuilder()
            .WithValidSections()
            .ForCurrentPeriod()
            .WithEmployerId("123")
            .WhereReportIsNotAlreadySubmitted()
            .Build();

        const string url = "home/index";
        UrlActionContext actualContext = null;
        _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

        var userModel = new UserModel();
        _mockUserService.Setup(s => s.GetUserModel(It.IsAny<ClaimsPrincipal>())).Returns(userModel).Verifiable();
        _reportService.Setup(s => s.GetReport("222", It.IsAny<string>())).ReturnsAsync(report).Verifiable();
        _reportService.Setup(s => s.CanBeEdited(report)).Returns(true).Verifiable();
        _reportService.Setup(s => s.SaveReport(report, userModel, null)).Callback<Report, UserModel, bool?>((r, u, k) => actualReport = r).Returns(() => Task.CompletedTask).Verifiable("Report was not saved");

        var sectionModel = new SectionModel
        {
            Id = "SubSectionOne",
            ReportingPeriod = "222",
            Questions =
            [
                new QuestionViewModel
                {
                    Id = "atEnd",
                    Answer = "123,000"
                },
                new QuestionViewModel
                {
                    Id = "atStart",
                    Answer = ""
                }
            ]
        };

        // act
        var result = await _controller.Submit(sectionModel);

        // assert
        _mockUrlHelper.VerifyAll();
        _reportService.VerifyAll();
        _mockUserService.VerifyAll();

        var redirectResult = result as RedirectResult;
        redirectResult.Should().NotBeNull();
        redirectResult.Url.Should().Be(url);
        actualContext.Action.Should().Be("Edit");
        actualContext.Controller.Should().Be("Report");

        actualReport.Should().NotBeNull();
        actualReport.IsValidForSubmission().Should().BeFalse();
    }
}