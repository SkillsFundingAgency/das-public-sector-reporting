using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PSRService.Web.DisplayText;
using SFA.DAS.PSRService.Web.ViewModels.Home;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests.Authorized_For_Edit_But_Not_Submit.No_Report;

[TestFixture]
public class WhenIRequestTheHomepage : And_No_Current_Report_Exists
{
    private IActionResult _result;
    private ViewResult _viewResult;
    private IndexViewModel _model;

    protected override async Task When()
    {
        _result = await Sut.Index();

        _viewResult = _result as ViewResult;
        _model = _viewResult?.Model as IndexViewModel;
    }

    [Test]
    public void Then_ViewResult_Is_Returned()
    {
        _viewResult = _result as ViewResult;
    }

    [Test]
    public void Then_ViewResult_Is_No_Null()
    {
        _viewResult.Should().NotBeNull();
    }

    [Test]
    public void Then_Model_Is_An_IndexViewModel()
    {
        _model = _viewResult.Model as IndexViewModel;
    }

    [Test]
    public void Then_Model_Is_Not_Null()
    {
        _model.Should().NotBeNull();
    }

    [Test]
    public void Then_Create_Report_Is_Enabled()
    {
        _model.CanCreateReport.Should().BeTrue();
    }

    [Test]
    public void Then_Edit_Report_Is_Disabled()
    {
        _model.CanEditReport.Should().BeFalse();
    }

    [Test]
    public void Then_Readonly_Is_False()
    {
        _model.Readonly.Should().BeFalse();
    }

    [Test]
    public void Then_CurrentReportAlreadySubmitted_Is_False()
    {
        _model.CurrentReportAlreadySubmitted.Should().BeFalse();
    }

    [Test]
    public void Then_The_Welcome_Message_Is_Edit_No_Report()
    {
        var expectedMessage = HomePageWelcomeMessageProvider
            .GetMessage()
            .ForPeriod(CurrentPeriod)
            .WhereUserCanEdit()
            .AndReportDoesNotExist();

        _model.WelcomeMessage.Should().BeEquivalentTo(expectedMessage);
    }
}