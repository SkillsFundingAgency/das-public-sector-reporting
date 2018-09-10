using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests.Authorized_For_Edit_But_Not_Submit.No_Report
{
    [TestFixture]
    public class And_No_Current_Report_Exists : And_User_Is_Authorized_For_Edit_But_Not_Submit
    {
        protected override void Given()
        {
            base.Given();

            _mockReportService.Setup(r => r.GetReport(CurrentPeriod.PeriodString, "ABCDE")).Returns((Report)null).Verifiable("Current report wasn't requested");

        }
    }
}
