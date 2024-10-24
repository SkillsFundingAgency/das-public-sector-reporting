namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report.
    And_I_Have_Reported_All_Mandatory_Factors;

public abstract class And_I_Have_Reported_All_Mandatory_Factors(bool isLocalAuthority) : Given_I_Have_Created_A_Report(isLocalAuthority)
{
    protected override async Task Given()
    {
        await base.Given();
        await BuildAndSubmitAllMandatoryFactors();
    }

    private async Task BuildAndSubmitAllMandatoryFactors()
    {
        await QuestionController.Submit(new FactorsAnswersBuilder().BuildValidChallengesAnswer().ForReportingPeriod(TestHelper.CurrentPeriod));
        await QuestionController.Submit(new FactorsAnswersBuilder().BuildValidOutlineActionsAnswer().ForReportingPeriod(TestHelper.CurrentPeriod));
        await QuestionController.Submit(new FactorsAnswersBuilder().BuildValidTargetPlansAnswer().ForReportingPeriod(TestHelper.CurrentPeriod));
    }
}