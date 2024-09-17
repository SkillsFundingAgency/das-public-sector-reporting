using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests;

[TestFixture]
public class Given_I_Request_The_Report_Edit_Page : ReportControllerTestBase
{
    [Test]
    public async Task The_Report_Exists_And_Is_Editable_Then_Show_Edit_Report()
    {
        // arrange
        MockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(CurrentValidNotSubmittedReport).Verifiable();
        MockReportService.Setup(s => s.CanBeEdited(CurrentValidNotSubmittedReport)).Returns(true).Verifiable();

        // act
        var result = await Controller.Edit();

        // assert
        MockUrlHelper.VerifyAll();
        MockReportService.VerifyAll();

        result.Should().BeOfType<ViewResult>();
        var editViewResult = result as ViewResult;
        editViewResult.Should().NotBeNull();
        editViewResult.ViewName.Should().Be("Edit", "View name does not match, should be: List");

        editViewResult.Model.Should().BeOfType<ReportViewModel>();
        var reportViewModel = editViewResult.Model as ReportViewModel;
        reportViewModel.Should().NotBeNull();
    }

    [Test]
    public async Task The_Report_Does_Not_Exist_Then_Should_Not_Error()
    {
        // arrange
        MockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns("report/create");
        MockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((Report)null).Verifiable();
        MockReportService.Setup(s => s.CanBeEdited(null)).Returns(false).Verifiable();

        // act
        var result =await Controller.Edit();

        // assert
        MockUrlHelper.VerifyAll();
        MockReportService.VerifyAll();
        result.Should().BeOfType<RedirectResult>();
    }

    [Test]
    public async Task The_Report_Exists_And_Not_Editable_Then_Redirect_Home()
    {
        // arrange
        const string url = "report/create";
        UrlActionContext actualContext = null;
        MockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");
        MockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(CurrentValidNotSubmittedReport).Verifiable();
        MockReportService.Setup(s => s.CanBeEdited(CurrentValidNotSubmittedReport)).Returns(false).Verifiable();

        // act
        var result = await Controller.Edit();

        // assert
        MockUrlHelper.VerifyAll();
        MockReportService.VerifyAll();

        var redirectResult = result as RedirectResult;
        redirectResult.Should().NotBeNull();
        redirectResult.Url.Should().Be(url);
        actualContext.Action.Should().Be("Index");
        actualContext.Controller.Should().Be("Home");
    }
}