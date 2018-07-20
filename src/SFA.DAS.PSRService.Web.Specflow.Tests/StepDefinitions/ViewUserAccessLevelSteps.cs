using System;
using NUnit.Framework;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
    [Binding]
    public class ViewUserAccessLevelSteps : BaseTest
    {
        [Then(@"the Home page should be displayed")]
        public void ThenTheUserIsReturnedToTheHomepage()
        {
            pageFactory.Homepage.Verify();
        }

        [Then(@"the View report details page is displayed")]
        public void ThenTheViewReportDetailsPageIsDisplayed()
        {
            pageFactory.ViewOnlyReportSummary.Verify();
        }

        [Then(@"View Report radio button does exist")]
        public void ThenViewReportRadioButtonDoesExist()
        {
            Assert.IsTrue(pageFactory.Homepage.DoesViewReportButtonExist);
        }

        [Then(@"Create Report radio button does not exist")]
        public void ThenCreateReportRadioButtonDoesNotExist()
        {
            Assert.IsFalse(pageFactory.Homepage.DoesCreateReportButtonExist);
        }

        [Then(@"the confirm submission button should not be available")]
        public void ThenTheConfirmSubmissionButtonShouldNotBeAvailable()
        {
            Assert.IsFalse(pageFactory.ViewOnlyReportSummary.DoesContinueButtonExist);
        }
    }
}
