using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification
{
    [ExcludeFromCodeCoverage]
    public class YourEmployeesVerifier : ReportNumbersVerifier
    {
        public YourEmployeesVerifier(string reportData) 
            : base(reportData)
        {
        }

        protected override string SectionName => "YourEmployees";
    }
}