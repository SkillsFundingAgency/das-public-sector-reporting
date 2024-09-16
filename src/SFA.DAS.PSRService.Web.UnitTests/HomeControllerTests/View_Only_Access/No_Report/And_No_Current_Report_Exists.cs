using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests.View_Only_Access.No_Report;

[TestFixture]
public class And_No_Current_Report_Exists : And_User_Is_Not_Authorized
{
    protected override void Given()
    {
        base.Given();

        MockReportService.Setup(r => r.GetReport(CurrentPeriod.PeriodString, "ABCDE")).Returns((Report)null).Verifiable("Current report wasn't requested");

    }
}