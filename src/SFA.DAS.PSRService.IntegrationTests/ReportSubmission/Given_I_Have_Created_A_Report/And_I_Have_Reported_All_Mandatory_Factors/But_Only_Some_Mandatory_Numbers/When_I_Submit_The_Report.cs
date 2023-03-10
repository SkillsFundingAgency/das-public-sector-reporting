using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report.
    And_I_Have_Reported_All_Mandatory_Factors.But_Only_Some_Mandatory_Numbers
{
    public sealed class When_I_Submit_The_Report
        : But_Only_Some_Mandatory_Numbers
    {
        private IActionResult submitResult;

        public When_I_Submit_The_Report() : base(false){}

        protected override void When()
        {
            try
            {
                submitResult = SUT.Submit();
            }
            catch (Exception) { }

        }

        [Test]
        public void Then_Report_Is_Not_Persisted_As_Submitted()
        {
            TestHelper
                .GetAllReports()
                .Should()
                .NotContain(report => report.Submitted == true);
        }
    }
}