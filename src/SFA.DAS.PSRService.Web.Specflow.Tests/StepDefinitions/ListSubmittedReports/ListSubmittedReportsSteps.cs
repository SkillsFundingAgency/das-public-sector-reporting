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
        [When(@"User clicks on homepage Continue button")]
        public void WhenUserClicksOnHomepageContinueButton()
        {
            PsrsHomepage homepage = new PsrsHomepage(webDriver);

            homepage.ClickContinueButton();
        }

        [When(@"I click on homepage Continue button")]
        public void WhenIClickOnContinueButton()
        {
            PsrsHomepage homepage = new PsrsHomepage(webDriver);

            homepage.ClickContinueButton();
        }

        [Then(@"User should be taken to the Submitted Reports page")]
        public void ThenUserShouldBeTakenToTheSubmittedReportsPage()
        {
            pageFactory
                .PreviouslySubmittedReports
                .Verify();
        }

        [Then(@"back link is shown as '(.*)'")]
        public void ThenBackLinkIsShownAs(string p0)
        {
            Assert.True("Back" == pageFactory.PreviouslySubmittedReports.BackButtonLink.Text);
        }

        [Then(@"the User should see the message '(.*)' on the previously submitted page")]
        public void ThenIShouldSeeTheMessageOnThePreviouslySubmittedPage(string p0)
        {
            var message = pageFactory.PreviouslySubmittedReports.NoSubmittedReportDisplayMessage;

            Assert
                .IsTrue(
                    message
                        .Displayed);

            Assert
                .AreEqual(
                    p0,
                    message.Text);
        }

        [Then(@"I should see one submitted report displayed in list")]
        public void ThenIShouldSeeOneSubmittedReportDisplayedInList()
        {
            Assert.True(pageFactory.PreviouslySubmittedReports.SubmittedReportDisplayed.Displayed);

            var listCount = pageFactory.PreviouslySubmittedReports.SubmittedReportListCount;
            Assert.AreEqual(1, listCount);
        }

        [When(@"user clicks the back button")]
        public void WhenUserClicksTheBackButton()
        {
            pageFactory.PreviouslySubmittedReports.BackButtonLink.Click();
        }

        [Then(@"the user is returned to the homepage")]
        public void ThenTheUserIsReturnedToTheHomepage()
        {
            pageFactory
                .Homepage
                .Verify();
        }
    }
}
