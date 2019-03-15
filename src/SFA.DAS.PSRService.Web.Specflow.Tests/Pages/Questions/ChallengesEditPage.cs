using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages.Questions
{
    public class ChallengesEditPage : FactorsQuestionEditPage
    {
        protected override string PAGE_TITLE => "What challenges have you faced this tax year in your efforts to meet the target, and how do these compare to the challenges experienced in 17/18";

        protected override string PageUrl => QuestionPageUrls.Challenges;

        public ChallengesEditPage(IWebDriver webDriver) : base(webDriver)
        {
        }
    }
}