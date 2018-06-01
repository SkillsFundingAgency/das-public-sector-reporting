using System;
using SFA.DAS.PSRService.Web.Specflow.Tests.Pages;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
    [Binding]
    public class ListSubmittedReportsSteps : HomepageSharedSteps
    {
        [Given(@"User navigates to Homepage")]
        public void GivenUserNavigatesToHomepage()
        {
           webDriver.Url = Configurator.GetConfiguratorInstance().GetBaseUrl();
        }
       
        [When(@"I click on Continue button")]
        public void WhenIClickOnContinueButton()
        {
            PsrsHomepage homepage = new PsrsHomepage(webDriver);

            homepage.ClickContinueButton();
        }
        
        [Then(@"I should be taken to the Submitted Reports page")]
        public void ThenIShouldBeOnSubmittedReportsPage()
        {
            var submittedReports = new PreviouslySubmittedReportsPage(webDriver);
        }

        [Given(@"There are no submitted reports")]
        public void GivenThereAreNoSubmittedReports()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"User navigates to Submitted reports page")]
        public void WhenUserNavigatesToSubmittedReportsPage()
        {
            webDriver.Url = Configurator.GetConfiguratorInstance().GetBaseUrl();

            var previouslySubmitted = new PreviouslySubmittedReportsPage(webDriver);
        }

        [Then(@"I should see the message '(.*)'")]
        public void ThenIShouldSeeNoSubmittedReportsMessage(string noReportsMessage)
        {
            var previouslySubmitted = new PreviouslySubmittedReportsPage(webDriver);

            previouslySubmitted.VerifyMessageDisplayed(noReportsMessage);

            
        }

        [Then(@"I should be taken to the Submitted Reports page")]
        public void ThenIShouldBeTakenToTheSubmittedReportsPage()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"back link is shown as '(.*)'")]
        public void ThenBackLinkIsShownAs(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I should see the message '(.*)'")]
        public void ThenIShouldSeeTheMessage(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"There is one submitted report")]
        public void GivenThereIsOneSubmittedReport()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I should see one submitted report displayed in list")]
        public void ThenIShouldSeeOneSubmittedReportDisplayedInList()
        {
            ScenarioContext.Current.Pending();
        }

    }
}
