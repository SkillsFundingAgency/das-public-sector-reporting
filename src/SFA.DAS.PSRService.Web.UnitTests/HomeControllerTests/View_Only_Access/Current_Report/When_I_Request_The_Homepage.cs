using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.DisplayText;
using SFA.DAS.PSRService.Web.ViewModels.Home;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests.View_Only_Access.Current_Report;

[TestFixture]
public class WhenIRequestTheHomepage : And_Current_Report_Exists
{
    private IActionResult _result;
    private ViewResult _viewResult;
    private IndexViewModel _model;

    protected override void When()
    {
        _result = Sut.Index();
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
    public void Then_Create_Report_Is_Disabled()
    {
        _model.CanCreateReport.Should().BeFalse();
    }

    [Test]
    public void Then_Edit_Report_Is_Disabled()
    {
        _model.CanEditReport.Should().BeFalse();
    }

    [Test]
    public void Then_Readonly_Is_True()
    {
        _model.Readonly.Should().BeTrue();
    }

    [Test]
    public void Then_CurrentReportAlreadySubmitted_Is_False()
    {
        _model.CurrentReportAlreadySubmitted.Should().BeFalse();
    }

    [Test]
    public void Then_The_Welcome_Message_Is_View_Only_Report_In_Progress()
    {
        var expectedMessage = HomePageWelcomeMessageProvider
                    .GetMesssage()
                    .ForPeriod(CurrentPeriod)
                    .WhereUserCanOnlyView()
                    .AndReportIsInProgress();

        _model
            .WelcomeMessage
            .Should()
            .BeEquivalentTo(
                expectedMessage);
    }
}