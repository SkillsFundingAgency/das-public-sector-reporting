namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report.And_I_Have_Reported_All_Mandatory_Factors.And_All_Mandatory_Numbers;

public abstract class And_All_Mandatory_Numbers(bool isLocalAuthority) : And_I_Have_Reported_All_Mandatory_Factors(isLocalAuthority)
{
    protected override async Task Given()
    {
        await base.Given();
        await BuildAndSubmitAllMandatoryNumbers();
    }

    private async Task BuildAndSubmitAllMandatoryNumbers()
    {
        await QuestionController.Submit(new ReportNumbersAnswersBuilder().BuildValidYourEmployeesAnswers().ForReportingPeriod(TestHelper.CurrentPeriod));
        await QuestionController.Submit(new ReportNumbersAnswersBuilder().BuildValidYourApprenticesAnswers().ForReportingPeriod(TestHelper.CurrentPeriod));
    }
}