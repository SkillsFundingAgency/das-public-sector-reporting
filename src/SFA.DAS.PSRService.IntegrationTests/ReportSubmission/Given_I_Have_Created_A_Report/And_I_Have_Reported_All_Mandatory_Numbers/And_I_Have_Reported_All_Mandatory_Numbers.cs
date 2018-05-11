namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report.And_I_Have_Reported_All_Mandatory_Numbers
{
    public abstract class And_I_Have_Reported_All_Mandatory_Numbers
    :Given_I_Have_Created_A_Report
    {
        protected override void Given()
        {
            base.Given();

            BuildAndSubmitAllMandatoryNumbers();
        }

        private void BuildAndSubmitAllMandatoryNumbers()
        {
            throw new System.NotImplementedException();
        }
    }
}