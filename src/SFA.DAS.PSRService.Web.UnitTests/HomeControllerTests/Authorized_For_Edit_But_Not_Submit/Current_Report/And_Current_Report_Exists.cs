﻿using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests.Authorized_For_Edit_But_Not_Submit.Current_Report;

[TestFixture]
public class And_Current_Report_Exists : And_User_Is_Authorized_For_Edit_But_Not_Submit
{
    protected override void Given()
    {
        base.Given();

        var report = new Report();
        MockReportService.Setup(r => r.GetReport(CurrentPeriod.PeriodString, "ABCDE")).ReturnsAsync(report).Verifiable("Current report wasn't requested");
    }
}