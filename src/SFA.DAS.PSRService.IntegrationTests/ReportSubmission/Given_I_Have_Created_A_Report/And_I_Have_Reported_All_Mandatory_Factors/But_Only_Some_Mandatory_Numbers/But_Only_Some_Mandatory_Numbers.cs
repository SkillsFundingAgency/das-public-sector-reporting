namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report.
    And_I_Have_Reported_All_Mandatory_Factors.But_Only_Some_Mandatory_Numbers
{
    public abstract class But_Only_Some_Mandatory_Numbers
        : And_I_Have_Reported_All_Mandatory_Factors
    {
        protected override void Given()
        {
            base.Given();

            BuildAndSubmitOnlyYourApprentciesNumbers();
        }

        private void BuildAndSubmitOnlyYourApprentciesNumbers()
        {
            QuestionController
                .Submit(
                    new ReportNumbersAnswersBuilder()
                        .BuildValidYourApprenticesAnswers()
                        .ForReportingPeriod(TestHelper.CurrentPeriod));
        }
    }
}