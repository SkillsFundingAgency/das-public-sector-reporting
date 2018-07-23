using System;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public class ReportConfirmationPage : BasePage
    {
        private static String PAGE_TITLE = "ave you checked everything is correct?";

        private readonly By _backButtonLink = By.ClassName("link-back");
        private readonly By _returnToYourReportButton = By.LinkText("Return to your report");
        private readonly By _submitYourReportButton = By.CssSelector(".button[value='Submit your report']");//"Submit your report");

        public ReportConfirmationPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public override void Navigate()
        {
            WebDriver.Url = GetPageUrl(PageUrls.ReportConfirmSubmision);
        }

        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(this.GetPageHeading(), PAGE_TITLE);
        }
        
        internal void ClickBackButtonLink()
        {
            FormCompletionHelper.ClickElement(_backButtonLink);
        }

        internal void ClickReturnToYourReport()
        {
            FormCompletionHelper.ClickElement(_returnToYourReportButton);
        }

        internal void ClickSubmitYourReport()
        {
            FormCompletionHelper.ClickElement(_submitYourReportButton);
        }
    }
}