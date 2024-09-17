using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests;

[TestFixture]
public class Given_I_Request_The_Report_Confirm_Page : ReportControllerTestBase
{
    [Test]
    public void When_The_Report_Is_Valid_To_Submit_Then_Show_Confirm_View()
    {
        // arrange
        var report = new ReportBuilder()
            .WithValidSections()
            .WithEmployerId("ABCDEF")
            .ForCurrentPeriod()
            .WhereReportIsNotAlreadySubmitted()
            .Build();

        MockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(report).Verifiable();
        MockReportService.Setup(s => s.CanBeEdited(report)).Returns(true).Verifiable();
        Controller.ObjectValidator = GetObjectValidator().Object;

        // act
        var result = Controller.Confirm();

        // assert
        result.Should().BeOfType<ViewResult>();
        (((ViewResult)result).ViewName is "Confirm" or null).Should().BeTrue();
        MockReportService.VerifyAll();
    }

    [Test]
    public void When_Valid_Report_Confirmed_Then_Submit()
    {
        // arrange
        var report = new Report(); // not submitted and empty sections, should be valid for submission

        MockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(report).Verifiable();
        MockReportService.Setup(s => s.SubmitReport(report)).Verifiable();
        Controller.ObjectValidator = GetObjectValidator().Object;

        // act
        var result = Controller.SubmitPost();

        // assert
        MockReportService.VerifyAll();
        result.Should().BeOfType<ViewResult>();
        ((ViewResult)result).ViewName.Should().Be("SubmitConfirmation");
    }

    private static Mock<IObjectModelValidator> GetObjectValidator()
    {
        var objectValidator = new Mock<IObjectModelValidator>();
        objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
            It.IsAny<ValidationStateDictionary>(),
            It.IsAny<string>(),
            It.IsAny<object>()));
        return objectValidator;
    }

    [Test]
    public void When_Unconfirmed_Report_Is_Not_Valid_To_Submit_Then_Redirect_To_Summary()
    {
        // arrange
        MockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(new Report()).Verifiable();
        Controller.ObjectValidator = GetFailingObjectValidator().Object;

        const string url = "report/create";
        UrlActionContext actualContext = null;
        MockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

        // act
        var result = Controller.Confirm();

        // assert
        MockReportService.VerifyAll();
        result.Should().BeOfType<RedirectResult>();
        actualContext.Should().NotBeNull();
        actualContext.Controller.Should().Be("Report");
        actualContext.Action.Should().Be("Summary");
    }

    private static Mock<IObjectModelValidator> GetFailingObjectValidator()
    {
        var objectValidator = new Mock<IObjectModelValidator>();
        objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
            It.IsAny<ValidationStateDictionary>(),
            It.IsAny<string>(),
            It.IsAny<object>())).Callback<ActionContext, ValidationStateDictionary, string, object>((a, d, s, o) => { a.ModelState.AddModelError("1", "error"); });
        return objectValidator;
    }

    [Test]
    public void When_Confirmed_Report_Is_Not_Valid_To_Submit_Then_Redirect_To_Summary()
    {
        // arrange
        MockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(new Report()).Verifiable();

        Controller.ObjectValidator = GetFailingObjectValidator().Object;

        const string url = "report/create";
        UrlActionContext actualContext = null;
        MockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

        // act
        var result = Controller.SubmitPost();

        // assert
        MockReportService.VerifyAll();
        result.Should().BeOfType<RedirectResult>();
        actualContext.Should().NotBeNull();
        actualContext.Controller.Should().Be("Report");
        actualContext.Action.Should().Be("Summary");
    }

    [Test]
    public void When_Unconfirmed_Report_Is_Not_Found_Then_Return_404()
    {
        // arrange
        MockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns((Report)null).Verifiable();

        // act
        var result = Controller.Confirm();

        // assert
        MockReportService.VerifyAll();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public void When_Confirmed_Report_Is_Not_Found_Then_Return_404()
    {
        // arrange
        MockReportService.Setup(s => s.GetReport(It.IsAny<string>(), "ABCDE")).Returns((Report)null).Verifiable();

        // act
        var result = Controller.SubmitPost();

        // assert
        MockReportService.VerifyAll();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public void When_The_Report_Is_Submitted_Redirect_To_Home()
    {
        // arrange
        const string url = "home/Index";
        UrlActionContext actualContext = null;

        MockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");


        var report =
            new ReportBuilder()
                .WithValidSections()
                .WithEmployerId("ABCDEF")
                .ForCurrentPeriod()
                .WhereReportIsAlreadySubmitted()
                .Build();

        MockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(report).Verifiable();
        MockReportService.Setup(s => s.CanBeEdited(report)).Returns(false).Verifiable();
        Controller.ObjectValidator = GetObjectValidator().Object;

        // act
        var result = Controller.Confirm();

        // assert
        result.Should().BeOfType<RedirectResult>();
        var redirectResult = result as RedirectResult;
        redirectResult.Should().NotBeNull();
        redirectResult.Url.Should().Be(url);
        actualContext.Action.Should().Be("Index");
    }
}