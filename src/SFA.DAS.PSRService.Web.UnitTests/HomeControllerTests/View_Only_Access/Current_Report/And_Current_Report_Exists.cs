﻿using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests.View_Only_Access.Current_Report
{
    [TestFixture]
    public class And_Current_Report_Exists : And_User_Is_Not_Authorized
    {
        protected override void Given()
        {
            base.Given();

            var report = new Report();
            _mockReportService.Setup(r => r.GetReport(CurrentPeriod.PeriodString, "ABCDE")).Returns(report).Verifiable("Current report wasn't requested");
        }
    }
}
