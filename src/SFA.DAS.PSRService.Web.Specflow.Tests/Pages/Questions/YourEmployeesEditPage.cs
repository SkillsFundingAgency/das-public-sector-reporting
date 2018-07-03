using System;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public class YourEmployeesPage : QuestionEditPage
    {
        private static String PAGE_TITLE = "YourEmployees";

        public YourEmployeesPage(IWebDriver webDriver) : base(webDriver)
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
        By atStart = By.Name("Section.Questions[0].Answer");
        By atEnd = By.Name("Section.Questions[1].Answer");
        By newThisPeriod = By.Name("Section.Questions[2].Answer");
        public void EditAtStartValue(string value)
        {
            
            FormCompletionHelper.EnterText(atStart,value);
        }
        public void EditAtEndValue(string value)
        {
           
            FormCompletionHelper.EnterText(atEnd, value);
        }
        public void EditAtNewThisPeriodValue(string value)
        {
            
            FormCompletionHelper.EnterText(newThisPeriod, value);
        }

        public void VerifyAtStartValue(string expected)
        {
            PageInteractionHelper.VerifyValue(atStart, expected);
        }
        public void VerifyAtEndValue(string expected)
        {
            PageInteractionHelper.VerifyValue(atEnd, expected);
        }
        public void VerifyNewThisPeriodValue(string expected)
        {
            PageInteractionHelper.VerifyValue(newThisPeriod, expected);
        }

    }
}