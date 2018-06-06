using System;
using SFA.DAS.PSRService.Web.Specflow.Tests.Pages;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
    [Binding]
    public class ReportSharedSteps : BaseTest
    {

        [Given(@"No current report exists")]
        public void GivenNoCurrentReportExists()
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

        [Given(@"I have a valid report")]
        public void GivenIHaveAValidReport()
        {
            ScenarioContext.Current.Pending();
        }
        [Given(@"the report has been submitted")]
        public void GivenTheReportHasBeenSubmitted()
        {
            ScenarioContext.Current.Pending();
        }

        
       




    }
}
