using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages.Questions
{
    public class TargetPlansEditPage : FactorsQuestionEditPage
    {
        protected override string PAGE_TITLE => "How are you planning to meet the target in future? What will you continue to do or do differently?";

        protected override string PageUrl => QuestionPageUrls.TargetPlans;

        public TargetPlansEditPage(IWebDriver webDriver) : base(webDriver)
        {
        }
    }
}