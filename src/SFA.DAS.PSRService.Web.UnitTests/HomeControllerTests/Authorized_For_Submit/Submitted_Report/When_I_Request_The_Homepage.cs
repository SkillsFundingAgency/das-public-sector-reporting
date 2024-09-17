using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.DisplayText;
using SFA.DAS.PSRService.Web.ViewModels.Home;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests.Authorized_For_Submit.Submitted_Report;

[TestFixture]
public class WhenIRequestTheHomepage : And_Current_Report_Submitted
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
        Assert.IsNotNull(_viewResult);
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
    public void Then_Report_Period_Matches_Current()
    {
        _model.Period.Should().Be(CurrentPeriod);
    }

    [Test]
    public void Then_Readonly_Is_False()
    {
        _model.Readonly.Should().BeFalse();
    }

    [Test]
    public void Then_CurrentReportAlreadySubmitted_Is_True()
    {
        _model.CurrentReportAlreadySubmitted.Should().BeTrue();
    }

    [Test]
    public void Then_The_Welcome_Message_Is_Submit_Report_Submitted()
    {
        var expectedMessage = HomePageWelcomeMessageProvider
                    .GetMesssage()
                    .ForPeriod(CurrentPeriod)
                    .WhereUserCanSubmit()
                    .AndReportIsAlreadySubmitted();

        _model
            .WelcomeMessage
            .Should()
            .BeEquivalentTo(expectedMessage);
    }
}