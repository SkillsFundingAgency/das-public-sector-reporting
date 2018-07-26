using System;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public class YourApprenticesEditPage : QuestionEditPage
    {
        private static String PAGE_TITLE = "Your apprentices";

        private readonly By _atStart = By.Name("Section.Questions[0].Answer");
        private readonly By _atEnd = By.Name("Section.Questions[1].Answer");
        private readonly By _newThisPeriod = By.Name("Section.Questions[2].Answer");

        public YourApprenticesEditPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public override void Navigate()
        {
            WebDriver.Url = GetPageUrl(QuestionPageUrls.YourApprentices);
        }

        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(this.GetPageHeading(), PAGE_TITLE);
        }
       
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