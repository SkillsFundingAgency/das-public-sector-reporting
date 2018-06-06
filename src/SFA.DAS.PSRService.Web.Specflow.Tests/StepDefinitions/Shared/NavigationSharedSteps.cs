using System;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using SFA.DAS.PSRService.Web.Specflow.Tests.Pages;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
    [Binding]
    public class NavigationSharedSteps : BaseTest
    {
        [Given(@"User navigates to Homepage")]
        public void GivenUserNavigatesToHomepage()
        {
            webDriver.Url = Configurator.GetConfiguratorInstance().GetBaseUrl();
        }
        [Given(@"User navigates to the Create report page")]
        public void GivenUserNavigatesToTheCreateReportPage()
        {
            webDriver.Url = Configurator.GetConfiguratorInstance().GetBaseUrl() +
                            Configurator.GetConfiguratorInstance().GetAccountId() +
                            PageUrls.ReportCreate;
        }

        [Given(@"User navigates to the Edit report page")]
        public void GivenUserNavigatesToTheEditReportPage()
        {
            webDriver.Url = Configurator.GetConfiguratorInstance().GetBaseUrl() +
                            Configurator.GetConfiguratorInstance().GetAccountId() +
                            PageUrls.ReportEdit;
        }

        [Given(@"User navigates to the '(.*)' question page")]
        public void GivenUserNavigatesToTheQuestionPage(string p0)
        {
            webDriver.Url = Configurator.GetConfiguratorInstance().GetBaseUrl() +
                            Configurator.GetConfiguratorInstance().GetAccountId() +
                            String.Format(PageUrls.QuestionEdit, p0);
        }

        [Given(@"I navigate to Review summary page")]
        public void GivenINavigateToReviewSummaryPage()
        {
            webDriver.Url = Configurator.GetConfiguratorInstance().GetBaseUrl() +
                            Configurator.GetConfiguratorInstance().GetAccountId() +
                            PageUrls.ReportSummary;
        }
        [Given(@"user navigates to confirm submission page")]
        public void GivenUserNavigatesToConfirmSubmissionPage()
        {
            webDriver.Url = Configurator.GetConfiguratorInstance().GetBaseUrl() +
                            Configurator.GetConfiguratorInstance().GetAccountId() +
                            PageUrls.ReportConfirmSubmision;
        }

        [Given(@"user navigates to previously submitted reports page")]
        public void GivenUserNavigatesToPreviouslySubmittedReportsPage()
        {
            webDriver.Url = Configurator.GetConfiguratorInstance().GetBaseUrl() +
                            Configurator.GetConfiguratorInstance().GetAccountId() +
                            PageUrls.ReportPreviouslySubmittedList;
        }
        [Given(@"User navigates to the Your employees question page")]
        public void GivenUserNavigatesToTheYourEmployeesQuestionPage()
        {
            webDriver.Url = Configurator.GetConfiguratorInstance().GetBaseUrl() +
                            Configurator.GetConfiguratorInstance().GetAccountId() + 
                            QuestionPAgeUrls.YourEmployees;
        }

    }
}
