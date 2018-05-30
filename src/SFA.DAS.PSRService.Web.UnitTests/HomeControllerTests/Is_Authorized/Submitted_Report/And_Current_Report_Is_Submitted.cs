﻿using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests.Is_Authorized.Submitted_Report
{
    [TestFixture]
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
