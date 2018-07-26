using NUnit.Framework;
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

        [Then(@"the Continue button is not displayed")]
        public void The0TheContinueButtonIsNotDisplayed()
        {
            Assert.False(pageFactory.ReportSummary.DoesContinueButtonExist);
        }

        [Then(@"the error summary is displayed at the top of the page")]
        public void ThenTheErrorSummaryIsDisplayedAtTheTopOfThePage()
        {
            Assert.True(pageFactory.ReportSummary.IsErrorSummaryDisplayed);
        }
        [Then(@"Reporting percentages employment starts is (.*)")]
        public void ThenReportingPercentagesEmploymentStartsIs(decimal expectedPercentage)
        {
            Assert.IsTrue(
                pageFactory
                    .ReportSummary
                    .ReportingPercentagesEmploymentStartsIs(expectedPercentage),
                $"Expected Employment Starts percentage to be: {expectedPercentage}");
        }

        [Then(@"Reporting percentages total head count is (.*)")]
        public void ThenReportingPercentagesTotalHeadCountIs(decimal expectedPercentage)
        {
            Assert.IsTrue(
                pageFactory
                    .ReportSummary
                    .ReportingPercentagesTotalHeadCountIs(expectedPercentage),
                $"Expected Total Head Count percentage to be: {expectedPercentage}");
        }

        [Then(@"Reporting percentages new this period is (.*)")]
        public void ThenReportingPercentagesNewThisPeriodIs(decimal expectedPercentage)
        {
            Assert.IsTrue(
                pageFactory
                    .ReportSummary
                    .ReportingPercentagesNewThisPeriodIs(expectedPercentage),
                $"Expected New This Period percentage to be: {expectedPercentage}");
        }
    }
}
