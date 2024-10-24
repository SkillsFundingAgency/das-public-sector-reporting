using System.Linq;
using System.Security.Claims;
using System.Text.Unicode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.PSRService.Domain;
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
        
        _employerIdentifier = new EmployerIdentifier { AccountId = "ABCDE", EmployerName = "EmployerName" };

        _employerAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(It.IsAny<HttpContext>()))
            .Returns(_employerIdentifier);
        _employerAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(null))
            .Returns(_employerIdentifier);

        _controller = new QuestionController(_reportService.Object, _employerAccountServiceMock.Object, null, _periodServiceMock.Object) { Url = _mockUrlHelper.Object };

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
        _reportService.Setup(s => s.GetReport("111", It.IsAny<string>())).ReturnsAsync((Report)null).Verifiable();
        _reportService.Setup(s => s.CanBeEdited(null)).Returns(false).Verifiable();

        // act
        var result = await _controller.Submit(new SectionModel { ReportingPeriod = "111" });

        // assert
        _mockUrlHelper.VerifyAll();
        _reportService.VerifyAll();

        var redirectResult = result as RedirectToActionResult;
        redirectResult.Should().NotBeNull();
        redirectResult.ActionName.Should().Be("Index");
        redirectResult.ControllerName.Should().Be("Home");
    }

    [Test]
    public async Task And_Report_Is_Not_Editable_Then_Redirect_Home()
    {
        // arrange
        var report = new Report();
        _reportService.Setup(s => s.GetReport("111", It.IsAny<string>())).ReturnsAsync(report).Verifiable();
        _reportService.Setup(s => s.CanBeEdited(report)).Returns(false).Verifiable();

        // act
        var result = await _controller.Submit(new SectionModel { ReportingPeriod = "111" });

        // assert
        _mockUrlHelper.VerifyAll();
        _reportService.VerifyAll();

        var redirectResult = result as RedirectToActionResult;
        redirectResult.Should().NotBeNull();
        redirectResult.ActionName.Should().Be("Index");
        redirectResult.ControllerName.Should().Be("Home");
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
        
        _reportService.Setup(s => s.GetReport("222", It.IsAny<string>())).ReturnsAsync(_currentValidAndNotSubmittedReport).Verifiable();
        _reportService.Setup(s => s.SaveReport(It.IsAny<Report>(), It.IsAny<UserModel>(), null)).Callback<Report, UserModel, bool?>((r, u, s) => actualReport = r).Returns(() => Task.CompletedTask).Verifiable("Report was not saved");
        _reportService.Setup(s => s.CanBeEdited(It.IsAny<Report>())).Returns(true);
        
        var sectionModel = new SectionModel
        {
            Id = "SubSectionOne",
            ReportingPeriod = "222",
            Questions =
            [
                new QuestionViewModel
                {
                    Id = QuestionIdentities.AtEnd,
                    Answer = "123,000"
                },
                new QuestionViewModel
                {
                    Id = QuestionIdentities.AtStart,
                    Answer = "123"
                }
            ]
        };

        // act
        var result = await _controller.Submit(sectionModel);

        // assert
        _reportService.VerifyAll();
        
        var redirectResult = result as RedirectToActionResult;
        redirectResult.Should().NotBeNull();
        redirectResult.ActionName.Should().Be("Edit");
        redirectResult.ControllerName.Should().Be("Report");

        actualReport.Should().NotBeNull();

        var section = actualReport.GetQuestionSection("SubSectionOne");

        section.Should().NotBeNull();
        section.Questions.Count().Should().Be(3);
        section.Questions.Single(q => q.Id == QuestionIdentities.AtStart).Answer.Should().Be("123");
        section.Questions.Single(q => q.Id == QuestionIdentities.AtEnd).Answer.Should().Be("123000");

        var originalNewThisPeriodAnswer = _currentValidAndNotSubmittedReport
            .GetQuestionSection("SubSectionOne")
            .Questions
            .Single(q => q.Id == QuestionIdentities.NewThisPeriod)
            .Answer;

        section.Questions.Single(q => q.Id == QuestionIdentities.NewThisPeriod).Answer.Should().Be(originalNewThisPeriodAnswer);
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
        
        _reportService.Setup(s => s.GetReport("222", It.IsAny<string>())).ReturnsAsync(report).Verifiable();
        _reportService.Setup(s => s.CanBeEdited(report)).Returns(true).Verifiable();
        _reportService.Setup(s => s.SaveReport(report, It.IsAny<UserModel>(), null)).Callback<Report, UserModel, bool?>((r, u, k) => actualReport = r).Returns(() => Task.CompletedTask).Verifiable("Report was not saved");

        var sectionModel = new SectionModel
        {
            Id = "SubSectionOne",
            ReportingPeriod = "222",
            Questions =
            [
                new QuestionViewModel
                {
                    Id = QuestionIdentities.AtEnd,
                    Answer = "123,000"
                },
                new QuestionViewModel
                {
                    Id = QuestionIdentities.AtStart,
                    Answer = ""
                }
            ]
        };

        // act
        var result = await _controller.Submit(sectionModel);

        // assert
        _mockUrlHelper.VerifyAll();
        _reportService.VerifyAll();
        
        var redirectResult = result as RedirectToActionResult;
        redirectResult.Should().NotBeNull();
        redirectResult.ActionName.Should().Be("Edit");
        redirectResult.ControllerName.Should().Be("Report");

        actualReport.Should().NotBeNull();
        actualReport.IsValidForSubmission().Should().BeFalse();
    }
}