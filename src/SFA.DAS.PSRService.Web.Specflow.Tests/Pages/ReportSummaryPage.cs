using System;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public class ReportSummaryPage : BasePage
    {
        private static String PAGE_TITLE = "Review details";

        public ReportSummaryPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public override void Navigate()
        {
            WebDriver.Url = GetPageUrl(PageUrls.ReportSummary);
        }
        
        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(this.GetPageHeading(), PAGE_TITLE);
        }

        public bool DoesContinueButtonExist => PageInteractionHelper.IsElementPresent(By.Id("report-summary-continue"));

        public bool IsOrganisationNameDisplayed => PageInteractionHelper.IsElementPresent(By.ClassName("task-list-section"));
    }
}