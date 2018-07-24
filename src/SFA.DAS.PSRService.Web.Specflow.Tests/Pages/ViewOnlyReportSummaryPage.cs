using System;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public class ViewOnlyReportSummaryPage : BasePage
    {
        private static String PAGE_TITLE = "View details";

        private readonly By _continueButton = By.Id("report-summary-continue");

        public ViewOnlyReportSummaryPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public bool DoesContinueButtonExist => PageInteractionHelper.IsElementPresent(_continueButton);

        public override void Navigate()
        {
            WebDriver.Url = GetPageUrl(PageUrls.ReportSummary);
        }

        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(this.GetPageHeading(), PAGE_TITLE);
        }
    }
}