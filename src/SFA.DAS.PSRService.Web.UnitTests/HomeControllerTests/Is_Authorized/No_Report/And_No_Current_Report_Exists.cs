using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests.Is_Authorized.No_Report
{
    public class And_No_Current_Report_Exists : And_Is_Authorized
    {
        protected override void Given()
        {
            base.Given();

            _mockReportService.Setup(r => r.GetReport(period, "ABCDE")).Returns((Report)null).Verifiable("Current report wasn't requested");

        }
    }
}
