using System;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public class ReportHistoryPage : BasePage
    {
        private static String PAGE_TITLE = "Report history";

        private readonly By _backButtonLink = By.ClassName("link-back");
        private readonly By _detailItem = By.TagName("details");

        public ReportHistoryPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public override void Navigate()
        {
            WebDriver.Url = GetPageUrl(PageUrls.ReportHistory);
        }

        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(this.GetPageHeading(), PAGE_TITLE);
        }

        internal void ClickBackButtonLink()
        {
            FormCompletionHelper.ClickElement(_backButtonLink);
        }

        public int DetailItemCount => PageInteractionHelper.CountElements(_detailItem);

        public void VerifyEmployeesAtStart(int index, String expected)
        {
            VerifyDetailItemField(index, "your-employees-at-start", expected);
        }

        private void VerifyDetailItemField(int index, string id, String expected)
        {
            PageInteractionHelper.VerifyText(
                By.CssSelector($"details:nth-of-type({index + 1}) #{id}"),
                expected);
        }
    }
}