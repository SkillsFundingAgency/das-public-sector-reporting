namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report.
    And_I_Have_Reported_All_Mandatory_Factors.But_Only_Some_Mandatory_Numbers;

public abstract class But_Only_Some_Mandatory_Numbers
    : And_I_Have_Reported_All_Mandatory_Factors
{
    protected But_Only_Some_Mandatory_Numbers(bool isLocalAuthority) : base(isLocalAuthority)
    {
    }

    protected override async Task Given()
    {
        await base.Given();
        await BuildAndSubmitOnlyYourApprentciesNumbers();
    }

    private async Task BuildAndSubmitOnlyYourApprentciesNumbers()
    {
        await QuestionController.Submit(new ReportNumbersAnswersBuilder()
            .BuildValidYourApprenticesAnswers()
            .ForReportingPeriod(TestHelper.CurrentPeriod));
    }
}