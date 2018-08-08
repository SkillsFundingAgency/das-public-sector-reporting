using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages.Questions
{
    public abstract class FactorsQuestionEditPage : QuestionEditPage
    {
        private readonly By _text = By.Name("Section.Questions[0].Answer");

        protected abstract string PAGE_TITLE { get; }

        protected FactorsQuestionEditPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public override void Navigate()
        {
            WebDriver.Url = GetPageUrl(PageUrl);
        }

        protected abstract string PageUrl { get; }

        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(this.GetPageHeading(), PAGE_TITLE);
        }
        
        public void EditText(string value)
        {
            FormCompletionHelper.EnterText(_text, value);
        }

        public void VerifyText(string expected)
        {
            PageInteractionHelper.VerifyValue(_text, expected);
        }
    }
}