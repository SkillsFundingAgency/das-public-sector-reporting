using System;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public class ViewOnlyReportSummaryPage : ReportSummaryPage
    {
        private static String PAGE_TITLE = "View details";

        public ViewOnlyReportSummaryPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public bool? DoesContinueButtonExist => PageInteractionHelper.IsElementPresent(By.Id("report-summary-continue"));

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