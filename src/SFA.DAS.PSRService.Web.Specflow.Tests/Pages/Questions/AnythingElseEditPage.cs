using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages.Questions
{
    public class AnythingElseEditPage : FactorsQuestionEditPage
    {
        protected override string PAGE_TITLE => "Do you have anything else you want to tell us? (optional)";

        protected override string PageUrl => QuestionPageUrls.AnythingElse;

        public AnythingElseEditPage(IWebDriver webDriver) : base(webDriver)
        {
        }
    }
}