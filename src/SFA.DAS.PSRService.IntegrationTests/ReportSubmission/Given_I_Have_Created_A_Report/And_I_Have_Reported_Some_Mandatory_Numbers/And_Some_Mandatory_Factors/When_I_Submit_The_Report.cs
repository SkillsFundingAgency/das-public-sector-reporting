using NUnit.Framework;

namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report.And_I_Have_Reported_Some_Mandatory_Numbers.And_Some_Mandatory_Factors
{
    public sealed class When_I_Submit_The_Report
    : And_Some_Mandatory_Factors
    {
        [Test]
        public void Then_Fails()
        {
            Assert.Fail();
        }
    }
}