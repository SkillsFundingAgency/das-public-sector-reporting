using System;
using System.Linq;
using NUnit.Framework;
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
        private readonly By _summaryItem = By.ClassName("summary");
        private readonly By _yourEmployeesAtStart = By.Id("your-employees-at-start");

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

        public int SummaryItemCount
        {
            get
            {
                PageInteractionHelper.TurnOffImplicitWaits();
                try
                {
                    var summaryItems = WebDriver.FindElements(_summaryItem);
                    return summaryItems?.Count ?? 0;
                }
                catch (NoSuchElementException)
                {
                    return 0;
                }
                finally
                {
                    PageInteractionHelper.TurnOnImplicitWaits();
                }
            }
        }

        public void VerifyEmployeesAtStart(int index, String expected)
        {
            PageInteractionHelper.TurnOffImplicitWaits();
            try
            {
                string value = null;
                var detailItems = WebDriver.FindElements(_yourEmployeesAtStart);
                if (detailItems != null && detailItems.Count > index)
                {
                    value = detailItems[index].Text;
                }

                Assert.AreEqual(expected, value);
            }
            catch (NoSuchElementException)
            {
            }
            finally
            {
                PageInteractionHelper.TurnOnImplicitWaits();
            }
        }
    }
}