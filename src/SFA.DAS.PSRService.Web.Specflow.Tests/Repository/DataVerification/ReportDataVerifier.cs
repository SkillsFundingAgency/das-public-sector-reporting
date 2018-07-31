namespace SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification
{
    public class ReportDataVerifier
    {
        private readonly string _reportData;

        public ReportDataVerifier(string reportData)
        {
            _reportData = reportData;
        }

        public YourEmployeesReportDataVerifier YourEmployees => new YourEmployeesReportDataVerifier(_reportData);
    }
}