using System;
using NUnit.Framework;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.Pages;
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
        
        [Given(@"the (.*), (.*) and (.*) have been edited")]
        //public void GivenTheAndHaveBeenEdited(string p0, string p1, string p2)
        //{
        //    ScenarioContext.Current.Pending();
        //}
       
        
        [When(@"User clicks on Start button")]
        public void WhenUserClicksOnStartButton()
        {
            pageFactory.ReportCreate.StartButton.Click();
        }
        
        [When(@"User navigates to the Edit report page")]
        public void WhenUserNavigatesToTheEditReportPage()
        {
            pageFactory.ReportEdit.Navigate();
        }
        
        
        [Then(@"the create report page is displayed")]
        public void ThenTheCreateReportPageIsDisplayed()
        {
            Assert.True("Before you start" == pageFactory.ReportCreate.ReportsPageBeforeYouStartHeader.Text);
        }
        
        [Then(@"New report is created")]
        public void ThenNewReportIsCreated()
        {
            Assert.True(pageFactory.ReportSummary.ReportSummaryOrganisationName.Displayed);
        }
        
        [Then(@"The '(.*)' question values are saved")]
        public void ThenTheQuestionValuesAreSaved(string p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"the Review report details page is displayed")]
        public void ThenTheReviewReportDetailsPageIsDisplayed()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"the continue button should not be available")]
        public void ThenTheContinueButtonShouldNotBeAvailable()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
