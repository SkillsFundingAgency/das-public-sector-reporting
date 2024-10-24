namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report.
    And_I_Have_Reported_Some_Mandatory_Numbers.And_Some_Mandatory_Factors;

public abstract class And_Some_Mandatory_Factors : And_I_Have_Reportd_Some_Mandatory_Numbers
{
    protected And_Some_Mandatory_Factors(bool isLocalAuthority) : base(isLocalAuthority)
    {
    }

    protected override async Task Given()
    {
        await base.Given();
        await BuildAndSubmitTargetPlansAndChallengesMandatoryFactors();
    }

    private async Task BuildAndSubmitTargetPlansAndChallengesMandatoryFactors()
    {
        await QuestionController.Submit(new FactorsAnswersBuilder()
            .BuildValidTargetPlansAnswer()
            .ForReportingPeriod(TestHelper.CurrentPeriod));

        await QuestionController.Submit(new FactorsAnswersBuilder()
            .BuildValidChallengesAnswer()
            .ForReportingPeriod(TestHelper.CurrentPeriod));
    }
}