using SFA.DAS.PSRService.Web.Specflow.Tests.Pages;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
    public class HomepageSharedSteps : BaseTest
    {
        [Given(@"User selects Homepage View a previously submitted report Radio button")]
        public void GivenUserSelectsHomepageViewAPreviouslySubmittedReportRadioButton()
        {
            new PsrsHomepage(webDriver)
                .SelectPreviouslySubmittedReports();
        }

        [Given(@"User selects Homepage Create a report Radio button")]
        public void GivenUserSelectsHomepageCreateAReportRadioButton()
        {
            new PsrsHomepage(webDriver)
                .SelectCreateReport();
        }

        [Then(@"the Home page should be displayed")]
        public void ThenTheUserIsReturnedToTheHomepage()
        {
            pageFactory.Homepage.Verify();
        }
    }
}
