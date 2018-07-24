using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
    [Binding]
    public class ReportSummaryScreenSteps : BaseTest
    {
        [When(@"user clicks the continue button")]
        public void WhenUserClicksTheSubmitYourReportButton()
        {
            pageFactory.ReportSummary.ClickContinue() ;
        }
    }
}
