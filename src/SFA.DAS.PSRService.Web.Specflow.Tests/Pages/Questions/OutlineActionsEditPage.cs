using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages.Questions
{
    public class OutlineActionsEditPage : FactorsQuestionEditPage
    {
        protected override string PAGE_TITLE => "What actions have you taken this tax year to meet the target, and how do these compare to the challenges experienced in 17/18?";

        protected override string PageUrl => QuestionPageUrls.OutlineActions;

        public OutlineActionsEditPage(IWebDriver webDriver) : base(webDriver)
        {
        }
    }
}