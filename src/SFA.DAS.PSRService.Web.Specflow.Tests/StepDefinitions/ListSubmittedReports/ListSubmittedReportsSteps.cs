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

        [When(@"user clicks the List Submitted Reports back button")]
        public void WhenUserClicksTheListSubmittedReportsBackButton()
        { 
            pageFactory.PreviouslySubmittedReports.ClickBackButtonLink();
        }

        [When(@"user clicks on View link for report (.*)")]
        public void WhenUserClicksOnViewLinkForReport(int reportNumber)
        {
            pageFactory.PreviouslySubmittedReports.ClickViewReportLink(reportNumber - 1);
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
            Assert.True(pageFactory.PreviouslySubmittedReports.VerifyBackButtonText(p0));
        }

        [Then(@"the User should see the message '(.*)' on the previously submitted page")]
        public void ThenTheUserShouldSeeTheMessageOnThePreviouslySubmittedPage(string p0)
        {
            Assert.True(pageFactory.PreviouslySubmittedReports.VerifyNoSubmittedReportDisplayMessageText(p0));
        }

        [Then(@"the user should see one submitted report displayed in list")]
        public void ThenTheUserShouldSeeOneSubmittedReportDisplayedInList()
        {
            Assert.True(pageFactory.PreviouslySubmittedReports.IsSubmittedReportDisplayed);

            var listCount = pageFactory.PreviouslySubmittedReports.SubmittedReportListCount;
            Assert.AreEqual(1, listCount);
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
