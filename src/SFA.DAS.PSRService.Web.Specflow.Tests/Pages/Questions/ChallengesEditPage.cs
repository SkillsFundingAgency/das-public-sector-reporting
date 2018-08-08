using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages.Questions
{
    public class ChallengesEditPage : FactorsQuestionEditPage
    {
        protected override string PAGE_TITLE => "Tell us about any challenges you have faced in your efforts to meet the target";

        protected override string PageUrl => QuestionPageUrls.Challenges;

        public ChallengesEditPage(IWebDriver webDriver) : base(webDriver)
        {
        }
    }
}