using NUnit.Framework;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
    [Binding]
    public class SuperUserAccessLevelSteps : BaseTest
    {
        [Then(@"the confirm submission page is displayed")]
        public void ThenTheConfirmSubmissionageIsDisplayed()
        {
            Assert.True(pageFactory.ReportConfirmation.Verify());
        }
    }
}
