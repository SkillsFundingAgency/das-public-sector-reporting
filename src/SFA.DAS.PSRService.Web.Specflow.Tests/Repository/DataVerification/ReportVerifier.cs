namespace SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification
{
    public class ReportVerifier
    {
        private readonly ReportDto _report;

        public ReportVerifier(ReportDto report)
        {
            _report = report;
        }

        public YourEmployeesVerifier YourEmployees => new YourEmployeesVerifier(_report.ReportingData);

        public YourApprenticesVerifier YourApprentices => new YourApprenticesVerifier(_report.ReportingData);

        internal static ReportVerifier VerifyReport(ReportDto report)
        {
            return new ReportVerifier(report);
        }
    }
}