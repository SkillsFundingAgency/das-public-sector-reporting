using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions.ListSubmittedReports
{
    [Binding]
    [Scope(Feature = "List Submitted Reports - MPD-1151", Scenario = "User on previously submitted reports page can click back button")]
    class UserOnPreviouslySubmittedReportsPageCanClickBackButton
    {
        [Given(@"Full access is granted")]
        public void GivenFullAccessIsGranted()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"A Current report exists")]
        public void GivenACurrentReportExists()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"the report has been submitted")]
        public void GivenTheReportHasBeenSubmitted()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"user navigates to previously submitted reports page")]
        public void GivenUserNavigatesToPreviouslySubmittedReportsPage()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"user clicks the back button")]
        public void WhenUserClicksTheBackButton()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the user is displayed the homepage")]
        public void ThenTheUserIsDisplayedTheHomepage()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
