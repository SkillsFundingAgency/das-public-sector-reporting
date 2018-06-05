using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests.Is_Authorized;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests.Is_Unauthorized.Submitted_Report
{
    public class And_Current_Report_Submitted : And_Is_Authorized
    {
        protected override void Given()
        {
            base.Given();

            var report = new Report { Submitted = true };
            _mockReportService.Setup(r => r.GetReport(period, "ABCDE")).Returns(report).Verifiable("Current report wasn't requested");
        }
    }
}
