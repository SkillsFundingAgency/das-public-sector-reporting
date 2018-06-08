using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions.ListSubmittedReports
{
    [Binding]
    [Scope(Feature = "List Submitted Reports - MPD-1151", Scenario = "User Views previously submitted reports when no reports submitted")]
    public class UserViewsPreviouslySubmittedReportsWhenNoReportsSubmitted
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

        [Given(@"the report hasnt been submitted")]
        public void GivenTheReportHasntBeenSubmitted()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"User navigates to Submitted reports page")]
        public void WhenUserNavigatesToSubmittedReportsPage()
        {
            webDriver.Url = Configurator.GetConfiguratorInstance().GetBaseUrl();

            var previouslySubmitted = new PreviouslySubmittedReportsPage(webDriver);
        }

        [Then(@"I should see the message There are currently no submitted reports to show")]
        public void ThenIShouldSeeNoSubmittedReportsMessage(string noReportsMessage)
        {
            var previouslySubmitted = new PreviouslySubmittedReportsPage(webDriver);

            previouslySubmitted.VerifyMessageDisplayed(noReportsMessage);

        }
    }
}
