﻿using NUnit.Framework;

namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report.And_I_Have_Reported_All_Mandatory_Factors.But_Only_Some_Mandatory_Numbers
{
    public sealed class When_I_Submit_The_Report
    :But_Only_Some_Mandatory_Numbers
    {
        [Test]
        public void Then_Fails()
        {
            Assert.Fail();
        }
    }
}