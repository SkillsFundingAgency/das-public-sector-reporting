using NUnit.Framework;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
    [Binding]
    public class EditUserAccessLevel_MPD_1131Steps : BaseTest
    {
        private IWebDriver webDriver;

        //[Given(@"User navigates to the '(.*)' question page")]
        //public void GivenUserNavigatesToTheQuestionPage(string p0)
        //{
        //    ScenarioContext.Current.Pending();
        //}
        
        [When(@"User clicks on Start button")]
        public void WhenUserClicksOnStartButton()
        {
            pageFactory.ReportCreate.ClickStartButton();
        }
        
        [When(@"User navigates to the Edit report page")]
        public void WhenUserNavigatesToTheEditReportPage()
        {
            pageFactory.ReportEdit.Navigate();
        }
        
        [Then(@"the create report page is displayed")]
        public void ThenTheCreateReportPageIsDisplayed()
        {
            Assert.True(pageFactory.ReportCreate.Verify());
        }
        
        [Then(@"New report is created")]
        public void ThenNewReportIsCreated()
        {
            Assert.True(pageFactory.ReportSummary.IsOrganisationNameDisplayed);
        }
         
        [Then(@"the Review report details page is displayed")]
        public void ThenTheReviewReportDetailsPageIsDisplayed()
        {
            pageFactory.ReportSummary.Verify();
        }

        [Then(@"the continue button should not be available")]
        public void ThenTheContinueButtonShouldNotBeAvailable()
        {
            Assert.IsFalse(pageFactory.ReportSummary.DoesContinueButtonExist);
        }

        [Then(@"the continue button should be available")]
        public void ThenTheContinueButtonShouldBeAvailable()
        {
            Assert.IsTrue(pageFactory.ReportSummary.DoesContinueButtonExist);
        }
    }
}
