using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
    [Binding]
    public class ReportHistorySteps : BaseTest
    {
        [When(@"User clicks the report history page back link")]
        public void WhenUserClicksTheReportHistoryPageBackLink()
        {
            pageFactory.ReportHistory.ClickBackButtonLink();
        }
    }
}
