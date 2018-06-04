using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report.
    And_I_Have_Reported_Some_Mandatory_Numbers.And_Some_Mandatory_Factors
{
    public sealed class When_I_Submit_The_Report
        : And_Some_Mandatory_Factors
    {
        private IActionResult submitResult;

        protected override void When()
        {
            try
            {
                submitResult = SUT.Submit();

            }
            catch (Exception)
            {
            }
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