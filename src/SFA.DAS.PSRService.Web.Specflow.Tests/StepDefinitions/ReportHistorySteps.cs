using NUnit.Framework;
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

        [Then(@"User sees '(.*)' summary entries in the history view")]
        public void ThenUserSeesSummaryEntriesInTheHistoryView(int expectedNumberOfEntries)
        {
            Assert.AreEqual(expectedNumberOfEntries, pageFactory.ReportHistory.DetailItemCount);
        }
        
        [Then(@"report number (.*) has the number of employees at period start as '(.*)'")]
        public void ThenTheSecondReportHasTheNumberOfEmployeesAtPeriodStartAs(int reportIndex, string expected)
        {
            pageFactory.ReportHistory.VerifyEmployeesAtStart(reportIndex - 1, expected);
        }
    }
}
