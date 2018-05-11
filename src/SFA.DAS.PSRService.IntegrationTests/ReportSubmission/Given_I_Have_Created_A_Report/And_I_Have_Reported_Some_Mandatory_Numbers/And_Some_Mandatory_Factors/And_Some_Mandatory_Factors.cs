namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report.And_I_Have_Reported_Some_Mandatory_Numbers.And_Some_Mandatory_Factors
{
    public abstract class And_Some_Mandatory_Factors
    : And_I_Have_Reportd_Some_Mandatory_Numbers
    {
        protected override void Given()
        {
            base.Given();

            BuildAndSubmitSomeMandatoryFactors();
        }

        private void BuildAndSubmitSomeMandatoryFactors()
        {
            throw new System.NotImplementedException();
        }
    }
}