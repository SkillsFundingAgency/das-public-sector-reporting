using System;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.Specflow.Tests.Pages;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
  
    public class ListSubmittedReportsSteps : BaseTest
    {
        //[Given(@"User navigates to Homepage")]
        //public void GivenUserNavigatesToHomepage()
        //{
            //ScenarioContext.Current.Pending();
       // }

        //[When(@"Selects Homepage '(.*)' Radio button")]
        //public void WhenSelectsHomepageRadioButton(string p0)
       // {
            //ScenarioContext.Current.Pending();
        //}

        [When(@"I click on homepage Continue button")]
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
            Assert.True("Back" == pageFactory.PreviouslySubmittedReports.BackButtonLink.Text);
        }
        [Then(@"I should see the message '(.*)' on the previously submitted page")]
        public void ThenIShouldSeeTheMessageOnThePreviouslySubmittedPage(string p0)
        {
            Assert.True(pageFactory.PreviouslySubmittedReports.NoSubmittedReportDisplayMessage.Displayed);
        }

        [Then(@"I should see one submitted report displayed in list")]
        public void ThenIShouldSeeOneSubmittedReportDisplayedInList()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"user clicks the back button")]
        public void WhenUserClicksTheBackButton()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the user is displayed the homepage")]
        public void ThenTheUserIsDisplayedTheHomepage()
        {
            ScenarioContext.Current.Pending();
        }


    }
}
