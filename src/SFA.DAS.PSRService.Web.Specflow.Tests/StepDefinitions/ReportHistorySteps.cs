using System;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.Specflow.Tests.Repository;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
    [Binding]
    public class ReportHistorySteps : BaseTest
    {
        private SQLReportRepository _reportRepository;

        public ReportHistorySteps(SQLReportRepository repository)
        {
            _reportRepository = repository;
        }

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

        [Given(@"(.*) minutes in the past user answers the Your Employees new at start question with (.*)")]
        public void GivenMinutesInThePastUserAnswersTheYourEmployeesNewAtStartQuestionWith(
            int minutesInPast,
            int numberOfEmployees)
        {
            pageFactory.QuestionYourEmployees.Navigate();

            pageFactory.QuestionYourEmployees.EditAtStartValue(numberOfEmployees.ToString());

            pageFactory.QuestionYourEmployees.SaveQuestionAnswers();

            var report = GetCurrentReport();

            report.UpdatedUtc = DateTime.UtcNow.AddMinutes(-1 * minutesInPast);

            _reportRepository
                .UpdateTime(report);
        }

        private ReportDto GetCurrentReport()
        {
            return
                _reportRepository
                    .GetReportWithId(
                        ScenarioContext
                            .Current
                            .Get<Guid>(
                                ContextKeys.CurrentReportID));
        }
    }
}
