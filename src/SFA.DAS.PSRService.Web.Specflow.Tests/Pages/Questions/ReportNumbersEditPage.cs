using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public abstract class ReportNumbersEditPage : QuestionEditPage
    {
        private readonly By _atStart = By.Name("Section.Questions[0].Answer");
        private readonly By _atEnd = By.Name("Section.Questions[1].Answer");
        private readonly By _newThisPeriod = By.Name("Section.Questions[2].Answer");

        protected ReportNumbersEditPage(IWebDriver webDriver) : base(webDriver)
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

        protected abstract string PAGE_TITLE { get; }

        public void EditAtStartValue(string value)
        {
            FormCompletionHelper.EnterText(_atStart,value);
        }

        public void EditAtEndValue(string value)
        {
           
            FormCompletionHelper.EnterText(_atEnd, value);
        }

        public void EditAtNewThisPeriodValue(string value)
        {
            
            FormCompletionHelper.EnterText(_newThisPeriod, value);
        }

        public void VerifyAtStartValue(string expected)
        {
            PageInteractionHelper.VerifyValue(_atStart, expected);
        }

        public void VerifyAtEndValue(string expected)
        {
            PageInteractionHelper.VerifyValue(_atEnd, expected);
        }

        public void VerifyNewThisPeriodValue(string expected)
        {
            PageInteractionHelper.VerifyValue(_newThisPeriod, expected);
        }
    }
}