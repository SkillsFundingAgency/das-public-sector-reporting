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

        [When(@"I Select Homepage '(.*)' Radio button")]
        public void WhenISelectHomepageRadioButton(string p0)
        {
            PsrsHomepage homepage = new PsrsHomepage(webDriver);

            switch (p0)
            {
                case "Create a new report":
                    homepage.SelectCreateReport();
                    break;
            }
        }


    }
}
