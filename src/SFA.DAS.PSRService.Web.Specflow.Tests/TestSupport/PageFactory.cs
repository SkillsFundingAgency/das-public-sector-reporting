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
        public ReportEditCompletePage ReportEditComplete => new ReportEditCompletePage(webDriver);
        public PreviouslySubmittedReportsPage PreviouslySubmittedReports => new PreviouslySubmittedReportsPage(webDriver);
        public ReportCreatePage ReportCreate => new ReportCreatePage(webDriver);
        public ReportSubmitConfirmationPage ReportSubmitConfirmation => new ReportSubmitConfirmationPage(webDriver);
        public ViewOnlyReportSummaryPage ViewOnlyReportSummary => new ViewOnlyReportSummaryPage(webDriver);
        public ReportAlreadySubmittedPage ReportAlreadySubmitted => new ReportAlreadySubmittedPage(webDriver);

        public ReportOrganisationNamePage ReportOrganisationName => new ReportOrganisationNamePage(webDriver);
        
        //Psrs Question pages
        public YourEmployeesEditPage QuestionYourEmployees => new YourEmployeesEditPage(webDriver);
        public YourApprenticesEditPage QuestionYourApprentices => new YourApprenticesEditPage(webDriver);
        public FullTimeEquivalentEditPage QuestionFullTimeEquivalent => new FullTimeEquivalentEditPage(webDriver);

        //Employer Idams pages
        public EmployerIdamsLoadingPage IdamsLoading => new EmployerIdamsLoadingPage(webDriver);
        public EmployerIdamsLoginPage IdamsLogin => new EmployerIdamsLoginPage(webDriver);
    }
}
