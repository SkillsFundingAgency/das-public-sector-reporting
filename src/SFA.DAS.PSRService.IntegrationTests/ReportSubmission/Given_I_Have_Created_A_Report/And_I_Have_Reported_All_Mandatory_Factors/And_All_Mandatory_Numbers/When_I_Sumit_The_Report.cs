using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report.And_I_Have_Reported_All_Mandatory_Factors.And_All_Mandatory_Numbers
{
    public sealed class When_I_Sumit_The_Report
    : And_All_Mandatory_Numbers
    {
        private IActionResult submitResponse;

        protected override void When()
        {
            submitResponse
                =
                SUT
                    .Submit(
                        TestHelper
                            .CurrentPeriod
                            .PeriodString);
        }

        [Test]
        public void Then_I_Am_Presented_With_The_Submitted_View()
        {
            Assert
                .IsInstanceOf<ViewResult>(
                    submitResponse);

            Assert
                .AreEqual(
                    "Submitted"
                    , ((ViewResult)submitResponse)
                    .ViewName);
        }
    }
}