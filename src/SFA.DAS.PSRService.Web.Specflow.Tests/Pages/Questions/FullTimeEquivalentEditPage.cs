using System;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages.Questions
{
    public class FullTimeEquivalentEditPage : QuestionEditPage
    {
        private static String PAGE_TITLE = "Full time equivalents";

        private readonly By _fullTimeEquivalents = By.Name("Section.Questions[0].Answer");

        private readonly By _atStart = By.Name("Section.Questions[0].Answer");

        public FullTimeEquivalentEditPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public override void Navigate()
        {
            WebDriver.Url = GetPageUrl(QuestionPageUrls.FullTimeEquivalent);
        }

        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(this.GetPageHeading(), PAGE_TITLE);
        }

        public void EditFullTimeEquivalents(string value)
        {
            FormCompletionHelper.EnterText(_fullTimeEquivalents,value);
        }

        public void VerifyFullTimeEquivalentsValue(string expected)
        {
            PageInteractionHelper.VerifyValue(_fullTimeEquivalents, expected);
        }

        public void EditAtStartValue(string value)
        {
            FormCompletionHelper.EnterText(_atStart, value);
        }

        public void VerifyAtStartValue(string expected)
        {
            PageInteractionHelper.VerifyValue(_atStart, expected);
        }
    }
}