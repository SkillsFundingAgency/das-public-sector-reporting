using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages.Questions
{
    public class YourEmployeesEditPage : ReportNumbersEditPage
    {
        public YourEmployeesEditPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        protected override string PageUrl => QuestionPageUrls.YourEmployees;

        protected override string PAGE_TITLE => "Your employees";
    }
}