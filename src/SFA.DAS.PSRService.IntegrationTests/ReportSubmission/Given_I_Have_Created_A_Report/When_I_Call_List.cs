using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report;

public sealed class When_I_Call_List : Given_I_Have_Created_A_Report
{
    private IActionResult _response;

    public When_I_Call_List() : base(false)
    {
    }

    protected override void When()
    {
        var hashedAccountId = "ABC123";
        _response = SUT.List(hashedAccountId);
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