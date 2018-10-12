using NUnit.Framework;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
    [Binding]
    public class HomepageSteps : BaseTest
    {
       [Given(@"User selects View report radio button")]
        public void GivenUserSelectsViewReportRadioButton()
        {
            pageFactory.Homepage.SelectViewReport();
        }

        [Given(@"User selects Edit report radio button")]
        public void GivenUserSelectsEditReportRadioButton()
        {
            pageFactory.Homepage.SelectEditReport();
        }

        [Then(@"report not yet created page is shown")]
        public void ThenReportNotYetCreatedPageIsShown()
        {
            Assert.IsTrue(pageFactory.ReportSummary.IsNoReportCreatedDisplayed);
        }

        [Then(@"report already submitted page is shown")]
        public void ThenReportAlreadySubmittedPageIsShown()
        {
            pageFactory.ReportAlreadySubmitted.Verify();
        }

        [When(@"User clicks the not yet created page back link")]
        public void WhenUserClicksTheNotYetCreatedPageBackLink()
        {
            pageFactory.ReportSummary.ClickBackButtonLink();
        }

        [When(@"User clicks the report already submitted page back link")]
        public void WhenUserClicksTheReportAlreadySubmittedPageBackLink()
        {
            pageFactory.ReportAlreadySubmitted.ClickBackButtonLink();
        }
    }
}
