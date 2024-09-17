using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.DisplayText;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Summary.Given_A_Valid_Not_Submitted_Report.
    And_User_Can_Edit_But_Not_Submit;

[ExcludeFromCodeCoverage]
[TestFixture]
public class WhenSummaryIsCalled : And_User_Can_Edit_But_Not_Submit
{
    private IActionResult _result;
    private ReportViewModel _model;

    protected override void When()
    {
        const string hashedAccountId = "ABC123";
        _result = Controller.Summary(hashedAccountId, "1718");

        var viewResult = _result as ViewResult;

        _model = viewResult?.Model as ReportViewModel;
    }

    [Test]
    public void Then_ViewModel_UserCanSubmitReports_Is_False()
    {
        _model.UserCanSubmitReports.Should().BeFalse();
    }

    [Test]
    public void Then_Result_Is_ViewResult()
    {
        _result.Should().NotBeNull();
        _result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void Then_ViewName_Is_Summary()
    {
        ((ViewResult)_result).ViewName.Should().Be("Summary", "View name does not match, should be: Summary");
    }

    [Test]
    public void Then_ViewModel_Is_ReportViewModel()
    {
        ((ViewResult)_result).Model.Should().NotBeNull();
        ((ViewResult)_result).Model.Should().BeOfType<ReportViewModel>();
    }

    [Test]
    public void Then_ViewModel_Has_Report()
    {
        var reportViewModel = ((ViewResult)_result).Model as ReportViewModel;

        reportViewModel.Report.Should().NotBeNull();
    }

    [Test]
    public void Then_Subtitle_Is_Appropriate_For_Edit_user()
    {
        var expectedText = SummaryPageMessageBuilder
            .GetSubtitle()
            .ForUserWhoCanEditButNotSubmit();

        _model.Subtitle
            .Should()
            .BeEquivalentTo(expectedText);
    }

    [Test]
    public void Then_ViewModel_IsReadOnly_Is_False()
    {
        _model.IsReadOnly
            .Should()
            .BeFalse();
    }
}