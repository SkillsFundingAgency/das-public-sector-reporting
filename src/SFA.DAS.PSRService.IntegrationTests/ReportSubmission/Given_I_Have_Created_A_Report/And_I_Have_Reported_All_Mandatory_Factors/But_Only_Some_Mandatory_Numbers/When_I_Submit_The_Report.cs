namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report.
    And_I_Have_Reported_All_Mandatory_Factors.But_Only_Some_Mandatory_Numbers;

public sealed class When_I_Submit_The_Report
    : But_Only_Some_Mandatory_Numbers
{
    public When_I_Submit_The_Report() : base(false)
    {
    }

    protected override Task When()
    {
        try
        {
            Sut.Submit();
        }
        catch (Exception)
        {
        }
        
        return Task.CompletedTask;
    }

    [Test]
    public void Then_Report_Is_Not_Persisted_As_Submitted()
    {
        TestHelper
            .GetAllReports()
            .Should()
            .NotContain(report => report.Submitted == true);
    }
}