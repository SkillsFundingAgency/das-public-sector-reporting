using NUnit.Framework;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
    [Binding]
    public class ReportConfirmationScreenSteps : BaseTest
    {
        [When(@"user clicks the confirm submission back button")]
        public void WhenUserClicksTheConfirmSubmissionBackButton()
        {
            pageFactory.ReportConfirmation.ClickBackButtonLink();
        }

        [When(@"user clicks the return to your report button")]
        public void WhenUserClicksTheReturnToYourReportButton()
        {
            pageFactory.ReportConfirmation.ClickReturnToYourReport();
        }

        [When(@"user clicks the submit your report button")]
        public void WhenUserClicksTheSubmitYourReportButton()
        {
            pageFactory.ReportConfirmation.ClickSubmitYourReport() ;
        }

        [Then(@"the confirm submission button should have text '(.*)'")]
        public void ThenTheConfirmSubmissionButtonShouldHaveText(string p0)
        {
            pageFactory.ReportConfirmation.VerifySubmitButtonText(p0);
        }
    }
}
