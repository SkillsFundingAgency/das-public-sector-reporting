using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests.Not_Authorized_For_Edit.Current_Report
{
    [TestFixture]
    public class And_Current_Report_Exists : And_User_Is_Not_Authorized
    {
        protected override void Given()
        {
            base.Given();

            var report = new Report();
            _mockReportService.Setup(r => r.GetReport(period, "ABCDE")).Returns(report).Verifiable("Current report wasn't requested");
        }
    }
}
