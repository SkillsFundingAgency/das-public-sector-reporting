using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests.Is_Unauthorized.Current_Report
{
    public class And_Current_Report_Exists : And_Is_Unauthorized
    {
        protected override void Given()
        {
            base.Given();

            var report = new Report();
            _mockReportService.Setup(r => r.GetReport(period, "ABCDE")).Returns(report).Verifiable("Current report wasn't requested");
        }
    }
}
