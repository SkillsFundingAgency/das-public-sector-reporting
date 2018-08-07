using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public class OutlineActionsEditPage : FactorsQuestionEditPage
    {
        protected override string PAGE_TITLE => "Outline any actions you have taken to help you progress towards meeting the public sector target";

        /*
AnythingElse
Do you have anything else you want to tell us? (optional)
*/
        protected override string PageUrl => QuestionPageUrls.OutlineActions;

        public OutlineActionsEditPage(IWebDriver webDriver) : base(webDriver)
        {
        }
    }
}