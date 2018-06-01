using System;
using SFA.DAS.PSRService.Web.Specflow.Tests.Pages;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
    [Binding]
    public class SpecFlowFeature1Steps : BaseTest
    {
        [Given(@"User navigates to Homepage")]
        public void GivenUserNavigatesToHomepage()
        {
           webDriver.Url = Configurator.GetConfiguratorInstance().GetBaseUrl();
        }

        [When(@"Selects '(.*)' Radio button")]
        public void WhenSelectsRadioButton(string p0)
        {
            PsrsHomepage homepage = new PsrsHomepage(webDriver);

            switch (p0)
            {
                case "View a previously submitted report":
                    homepage.SelectPreviouslySubmittedReports();
                    break;
            }
        }


        [When(@"I click on Continue button")]
        public void WhenIClickOnContinueButton()
        {
            PsrsHomepage homepage = new PsrsHomepage(webDriver);

            homepage.ClickContinueButton();
        }
        
        [Then(@"I should be on Submitted Reports page")]
        public void ThenIShouldBeOnSubmittedReportsPage()
        {
            var submittedReports = new PreviouslySubmittedReportsPage(webDriver);
        }
    }
}
