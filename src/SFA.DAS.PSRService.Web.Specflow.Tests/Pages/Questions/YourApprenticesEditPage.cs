using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages.Questions
{
    public class YourApprenticesEditPage : ReportNumbersEditPage
    {
        public YourApprenticesEditPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        protected override string PageUrl => QuestionPageUrls.YourApprentices;

        protected override string PAGE_TITLE => "Your apprentices";
    }
}