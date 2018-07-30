using System;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public class ReportEditCompletePage : BasePage
    {
        private static String PAGE_TITLE = "Report not submitted yet";

        public ReportEditCompletePage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public override void Navigate()
        {
            WebDriver.Url = GetPageUrl(PageUrls.ReportEditComplete);
        }

        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(this.GetPageHeading(), PAGE_TITLE);
        }
    }
}