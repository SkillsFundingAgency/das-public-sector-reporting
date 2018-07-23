using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.Pages;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport
{
    public class PageFactory
    {
        private readonly IWebDriver webDriver;

        public PageFactory(IWebDriver driver)
        {
            this.webDriver = driver;
        }

        //PSRS pages
        public PsrsHomepage Homepage => new PsrsHomepage(webDriver);
        public ReportEditPage ReportEdit => new ReportEditPage(webDriver);
        public ReportSummaryPage ReportSummary => new ReportSummaryPage(webDriver);
        public ReportConfirmationPage ReportConfirmation => new ReportConfirmationPage(webDriver);
        public PreviouslySubmittedReportsPage PreviouslySubmittedReports => new PreviouslySubmittedReportsPage(webDriver);
        public ReportCreatePage ReportCreate => new ReportCreatePage(webDriver);
        public ReportSubmitConfirmationPage ReportSubmitConfirmation => new ReportSubmitConfirmationPage(webDriver);
        public ViewOnlyReportSummaryPage ViewOnlyReportSummary => new ViewOnlyReportSummaryPage(webDriver);

        //Psrs Question pages
        public YourEmployeesPage QuestionYourEmployees => new YourEmployeesPage(webDriver);

        //Employer Idams pages
        public EmployerIdamsLoadingPage IdamsLoading => new EmployerIdamsLoadingPage(webDriver);
        public EmployerIdamsLoginPage IdamsLogin => new EmployerIdamsLoginPage(webDriver);
    }
}
