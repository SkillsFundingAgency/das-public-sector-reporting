﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NUnit.Framework;

namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report.
    And_I_Have_Reported_All_Mandatory_Factors.And_All_Mandatory_Numbers
{
    public sealed class When_I_Submit_The_Report
        : And_All_Mandatory_Numbers
    {
        private IActionResult submitResponse;

        public When_I_Submit_The_Report() : base(false){}
   
        protected override void When()
        {
            submitResponse = SUT.SubmitPost();
        }

        [Test]
        public void Then_I_Am_Presented_With_The_SubmitConfirmation_View()
        {
            Assert
                .AreEqual(
                    "SubmitConfirmation"
                    , ((ViewResult) submitResponse).ViewName);
        }

        [Test]
        public void Then_Report_Is_Persisted_As_Submitted()
        {
            TestHelper
                .GetAllReports()
                .Should()
                .OnlyContain(report => report.Submitted == true);
        }
    }
}