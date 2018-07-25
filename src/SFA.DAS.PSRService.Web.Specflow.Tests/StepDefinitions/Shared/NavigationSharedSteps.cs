﻿using System;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using SFA.DAS.PSRService.Web.Specflow.Tests.Pages;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
    [Binding]
    public class NavigationSharedSteps : BaseTest
    {
        [Given(@"User navigates to Homepage")]
        public void GivenUserNavigatesToHomepage()
        {
            pageFactory.Homepage.Navigate();
        }

        [When(@"User navigates to Homepage")]
        public void WhenUserNavigatesToHomepage()
        {
            pageFactory.Homepage.Navigate();
        }

        [When(@"User clicks on homepage Continue button")]
        public void WhenUserClicksOnHomepageContinueButton()
        {
            pageFactory.Homepage.ClickContinueButton();
        }
        
        [Given(@"User navigates to the Create report page")]
        public void GivenUserNavigatesToTheCreateReportPage()
        {
            pageFactory.ReportCreate.Navigate();
        }

        [Given(@"User navigates to the Edit report page")]
        public void GivenUserNavigatesToTheEditReportPage()
        {
            pageFactory.ReportEdit.Navigate();
        }

        [Given(@"User navigates to Review summary page")]
        public void GivenUserNavigatesToReviewSummaryPage()
        {
            pageFactory.ReportSummary.Navigate();
        }

        [When(@"User navigates to Review summary page")]
        public void WhenUserNavigatesToReviewSummaryPage()
        {
            pageFactory.ReportSummary.Navigate();
        }

        [Given(@"user navigates to confirm submission page")]
        public void GivenUserNavigatesToConfirmSubmissionPage()
        {
            pageFactory.ReportConfirmation.Navigate();
        }

        [Given(@"user navigates to previously submitted reports page")]
        public void GivenUserNavigatesToPreviouslySubmittedReportsPage()
        {
            pageFactory.PreviouslySubmittedReports.Navigate();
        }

        [Given(@"User navigates to the Your employees question page")]
        public void GivenUserNavigatesToTheYourEmployeesQuestionPage()
        {
            pageFactory.QuestionYourEmployees.Navigate();
        }

        [Given(@"User navigates to the Your apprentices question page")]
        public void GivenUserNavigatesToTheYourApprenticesQuestionPage()
        {
            pageFactory.QuestionYourApprentices.Navigate();
        }

        [Given(@"User navigates to the Full time equivalent question page")]
        public void GivenUserNavigatesToTheFullTimeEquivalentQuestionPage()
        {
            pageFactory.QuestionFullTimeEquivalent.Navigate();
        }
    }
}
