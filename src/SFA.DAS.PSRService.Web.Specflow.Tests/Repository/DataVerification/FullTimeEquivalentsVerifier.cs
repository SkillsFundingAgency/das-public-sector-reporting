namespace SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification
{
    public class FullTimeEquivalentsVerifier : ReportNumbersVerifier
    {
        public FullTimeEquivalentsVerifier(string reportData) : base(reportData)
        {
        }

        protected override string SectionName => "FullTimeEquivalent";
    }
}