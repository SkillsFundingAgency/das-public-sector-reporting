using System;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public class FullTimeEquivalentEditPage : QuestionEditPage
    {
        private static String PAGE_TITLE = "Full time equivalents";

        public FullTimeEquivalentEditPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public override void Navigate()
        {
            WebDriver.Url = GetPageUrl(QuestionPageUrls.YourEmployees);
        }
        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(this.GetPageHeading(), PAGE_TITLE);
        }

        By fullTimeEquivalents = By.Name("Section.Questions[0].Answer");

        public void EditFullTimeEquivalents(string value)
        {
            FormCompletionHelper.EnterText(fullTimeEquivalents,value);
        }
        
        public void VerifyFullTimeEquivalentsValue(string expected)
        {
            PageInteractionHelper.VerifyValue(fullTimeEquivalents, expected);
        }
    }
}