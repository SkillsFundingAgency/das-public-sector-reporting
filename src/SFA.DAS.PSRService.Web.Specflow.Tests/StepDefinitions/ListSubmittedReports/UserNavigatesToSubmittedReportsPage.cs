using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions.ListSubmittedReports
{
    [Binding]
    [Scope(Feature = "List Submitted Reports - MPD-1151", Scenario = "User Navigates to submitted reports page")]
    public class UserNavigatesToSubmittedReportsPage
    {
        [Given(@"Full access is granted")]
        public void GivenFullAccessIsGranted()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"User navigates to Homepage")]
        public void GivenUserNavigatesToHomepage()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"Selects Homepage '(.*)' Radio button")]
        public void WhenSelectsHomepageRadioButton(string p0)
        {
            ScenarioContext.Current.Pending();
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

        [Then(@"back link is shown as '(.*)'")]
        public void ThenBackLinkIsShownAs(string p0)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
