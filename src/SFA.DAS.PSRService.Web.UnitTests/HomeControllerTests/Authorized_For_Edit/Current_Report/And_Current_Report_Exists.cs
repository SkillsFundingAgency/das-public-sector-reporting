using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests.Authorized_For_Edit.Current_Report
{
    [TestFixture]
    public class And_Current_Report_Exists : And_User_Is_Authorized_For_Edit
    {
        protected override void Given()
        {
            base.Given();

            var report = new Report();
            _mockReportService.Setup(r => r.GetReport(period, "ABCDE")).Returns(report).Verifiable("Current report wasn't requested");
        }
    }
}
