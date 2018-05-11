namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report.And_I_Have_Reported_All_Mandatory_Factors
{
    public abstract class And_I_Have_Reported_All_Mandatory_Factors
    : Given_I_Have_Created_A_Report
    {
        protected override void Given()
        {
            base.Given();

            BuildAndSubmitAllMandatoryFactors();
        }

        private void BuildAndSubmitAllMandatoryFactors()
        {
            throw new System.NotImplementedException();
        }
    }
}