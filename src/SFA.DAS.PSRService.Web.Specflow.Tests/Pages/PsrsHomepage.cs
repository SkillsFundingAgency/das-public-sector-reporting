using System;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public class PsrsHomepage : BasePage
    {
        private static String PAGE_TITLE = "Annual apprenticeship return";

        public PsrsHomepage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public override void Navigate()
        {
            WebDriver.Url = GetPageUrl(PageUrls.Home);
        }
        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(this.GetPageHeading(), PAGE_TITLE);
        }

        private readonly By _selectPageForm = By.CssSelector("label");
        private readonly By _submitPageFormButton = By.Id("SubmitSelectOptionForm");

        internal void SelectPreviouslySubmittedReports()
        {
            FormCompletionHelper.SelectRadioOptionById("home-action-list");
        }

        internal void SelectEditReport()
        {
            FormCompletionHelper.SelectRadioOptionById("home-action-edit");
        }

        internal void SelectCreateReport()
        {
            FormCompletionHelper.SelectRadioOptionById("home-action-create");
        }

        internal PreviouslySubmittedReportsPage ClickContinueButton()
        {
            FormCompletionHelper.ClickElement(_submitPageFormButton);
            return new PreviouslySubmittedReportsPage(WebDriver);
        }

        public IWebElement DisplayPsrsHomepageMenu => WebDriver.WaitForElementToBeVisible(By.ClassName("heading-xlarge"));
    }
}