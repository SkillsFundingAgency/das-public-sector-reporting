using System;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public class ReportAlreadySubmittedPage : BasePage
    {
        private static String PAGE_TITLE = "Report submitted";

        private readonly By _backButtonLink = By.ClassName("link-back");

        public ReportAlreadySubmittedPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public override void Navigate()
        {
            WebDriver.Url = GetPageUrl(PageUrls.ReportAlreadySubmitted);
        }

        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(this.GetPageHeading(), PAGE_TITLE);
        }

        internal void ClickBackButtonLink()
        {
            FormCompletionHelper.ClickElement(_backButtonLink);
        }
    }
}
