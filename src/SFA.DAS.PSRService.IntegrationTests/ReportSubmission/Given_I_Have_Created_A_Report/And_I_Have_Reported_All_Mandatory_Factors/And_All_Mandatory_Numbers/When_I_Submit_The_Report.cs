using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NUnit.Framework;

namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report.
    And_I_Have_Reported_All_Mandatory_Factors.And_All_Mandatory_Numbers;

public sealed class WhenISubmitTheReport : And_All_Mandatory_Numbers
{
    private IActionResult _submitResponse;

    public WhenISubmitTheReport() : base(false)
    {
    }

    protected override void When()
    {
        _submitResponse = SUT.SubmitPost();
    }

    [Test]
    public void Then_I_Am_Presented_With_The_SubmitConfirmation_View()
    {
        ((ViewResult)_submitResponse).ViewName.Should().Be("SubmitConfirmation");
    }

    [Test]
    public void Then_Report_Is_Persisted_As_Submitted()
    {
        TestHelper
            .GetAllReports()
            .Should()
            .OnlyContain(report => report.Submitted == true);
    }
}