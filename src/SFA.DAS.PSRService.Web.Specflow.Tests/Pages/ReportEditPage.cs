using System;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public class ReportEditPage : BasePage
    {
        private static String PAGE_TITLE = "Reporting your progress towards the public sector apprenticeship target";

        public ReportEditPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public override void Navigate()
        {
            WebDriver.Url = GetPageUrl(PageUrls.ReportEdit);
        }
        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(this.GetPageHeading(), PAGE_TITLE);
        }

        public void ClicksQuestionLink(string linkText)
        {
            FormCompletionHelper.ClickElement(By.LinkText(linkText));
        }

        public void ClicksReportSummaryLink(string linkText)
        {

        }

        
        
        
    }
}