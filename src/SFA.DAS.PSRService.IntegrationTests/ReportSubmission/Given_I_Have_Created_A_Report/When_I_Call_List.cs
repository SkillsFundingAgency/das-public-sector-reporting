using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report;

public sealed class When_I_Call_List() : Given_I_Have_Created_A_Report(false)
{
    private IActionResult _response;

    protected override async Task When()
    {
        const string hashedAccountId = "ABC123";
        _response = await Sut.List(hashedAccountId);
    }

    [Test]
    public void Then_Response_Is_Not_Null()
    {
        _response.Should().NotBeNull();
    }

    [Test]
    public void Then_Response_Is_A_ViewResult()
    {
        _response.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void Then_Response_Model_Is_A_ReportListViewModel()
    {
        ((ViewResult)_response).Model.Should().BeOfType<ReportListViewModel>();
    }

    [Test]
    public void Then_There_Are_No_Submitted_Reports()
    {
        var model = ((ViewResult)_response).Model as ReportListViewModel;
        model.SubmittedReports.Should().BeEmpty();
    }
}