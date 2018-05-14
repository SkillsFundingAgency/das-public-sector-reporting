namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report.And_I_Have_Reported_Some_Mandatory_Numbers.And_Some_Mandatory_Factors
{
    public abstract class And_Some_Mandatory_Factors
    : And_I_Have_Reportd_Some_Mandatory_Numbers
    {
        protected override void Given()
        {
            base.Given();

            BuildAndSubmitTargetPlansAndChallengesMandatoryFactors();
        }

        private void BuildAndSubmitTargetPlansAndChallengesMandatoryFactors()
        {
            QuestionController
                .Submit(
                    new FactorsAnswersBuilder()
                        .BuildValidTargetPlansAnswer()
                        .ForReportingPeriod(
                            TestHelper
                                .CurrentPeriod));

            QuestionController
                .Submit(
                    new FactorsAnswersBuilder()
                        .BuildValidChallengesAnswer()
                        .ForReportingPeriod(
                            TestHelper
                                .CurrentPeriod));
        }
    }
}