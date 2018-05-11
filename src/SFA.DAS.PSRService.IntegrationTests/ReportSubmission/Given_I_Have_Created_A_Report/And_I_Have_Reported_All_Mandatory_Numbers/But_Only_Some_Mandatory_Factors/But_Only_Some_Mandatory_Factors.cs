namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report.And_I_Have_Reported_All_Mandatory_Numbers.But_Only_Some_Mandatory_Factors
{
    public abstract class But_Only_Some_Mandatory_Factors
    :And_I_Have_Reported_All_Mandatory_Numbers
    {
        protected override void Given()
        {
            base.Given();

            BuildAndSubmitOnlySomeMandatoryFactors();
        }

        private void BuildAndSubmitOnlySomeMandatoryFactors()
        {
            throw new System.NotImplementedException();
        }
    }
}