using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public class TargetPlansEditPage : FactorsQuestionEditPage
    {
        protected override string PAGE_TITLE => "How are you planning to ensure you meet the target in future?";

        protected override string PageUrl => QuestionPageUrls.TargetPlans;

        public TargetPlansEditPage(IWebDriver webDriver) : base(webDriver)
        {
        }
    }
}