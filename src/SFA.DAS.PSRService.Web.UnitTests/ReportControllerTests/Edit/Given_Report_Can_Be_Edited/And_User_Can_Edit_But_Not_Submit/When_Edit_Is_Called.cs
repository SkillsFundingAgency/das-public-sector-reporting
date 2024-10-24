using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Edit.Given_Report_Can_Be_Edited.And_User_Can_Edit_But_Not_Submit;

public sealed class WhenEditIsCalled : And_User_Can_Edit_But_Not_Submit
{
    private IActionResult _result;
    private ReportViewModel _model;

    protected override async Task When()
    {
       await base.When();

        _result = await Sut.Edit();

        var viewResult = _result as ViewResult;

        _model = viewResult?.Model as ReportViewModel;
    }

    [Test]
    public void Then_ViewModel_UserCanSubmitReports_Is_False()
    {
        _model
            .UserCanSubmitReports
            .Should()
            .BeFalse();
    }

    [Test]
    public void Then_Result_Is_ViewResult()
    {
        _result.Should().NotBeNull();

        _result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void Then_ViewName_Is_Edit()
    {
        ((ViewResult)_result).ViewName.Should().Be("Edit", "View name does not match, should be: Summary");
    }

    [Test]
    public void Then_ViewModel_Is_ReportViewModel()
    {
        ((ViewResult)_result).Model.Should().NotBeNull();
        ((ViewResult)_result).Model.Should().BeOfType<ReportViewModel>();
    }
}