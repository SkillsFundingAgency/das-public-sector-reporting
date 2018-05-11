using NUnit.Framework;

namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report.And_I_Have_Reported_All_Mandatory_Factors.And_All_Mandatory_Numbers
{
    public sealed class When_I_Sumit_The_Report
    : And_All_Mandatory_Numbers
    {
        [Test]
        public void Then_Fails()
        {
            Assert.Fail();
        }
    }
}