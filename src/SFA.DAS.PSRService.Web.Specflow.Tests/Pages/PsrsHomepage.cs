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


        private readonly By _submitPageFormButton = By.Id("SubmitSelectOptionForm");
        private readonly By _signInButton = By.CssSelector("h1");

        public PsrsHomepage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public override void Navigate()
        {
            WebDriver.Url = GetPageUrl(PageUrls.Home);
        }

        public override bool Verify()
        {
            //Make sure homepage is loaded - can be a little slow when logging in
            PageInteractionHelper.WaitForPageToLoad();

            return PageInteractionHelper.VerifyPageHeading(this.GetPageHeading(), PAGE_TITLE);
        }

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

        internal void SelectViewReport()
        {
            FormCompletionHelper.SelectRadioOptionById("home-action-view");
        }

        internal void SelectListSubmittedReports()
        {
            FormCompletionHelper.SelectRadioOptionById("home-action-list");
        }

        internal PreviouslySubmittedReportsPage ClickContinueButton()
        {
            FormCompletionHelper.ClickElement(_submitPageFormButton);
            return new PreviouslySubmittedReportsPage(WebDriver);
        }

        internal bool IsPsrsHomepageMenuDisplayed => PageInteractionHelper.IsElementPresent(By.ClassName("heading-xlarge"));

        internal bool DoesCreateReportButtonExist => PageInteractionHelper.IsElementPresent(By.Id("home-action-create"));

        internal bool DoesEditReportButtonExist => PageInteractionHelper.IsElementPresent(By.Id("home-action-edit"));

        internal bool DoesViewReportButtonExist => PageInteractionHelper.IsElementPresent(By.Id("home-action-view"));

        public bool IsSignInDisplayed => PageInteractionHelper.IsElementPresent(_signInButton)
                                         && PageInteractionHelper.VerifyText(_signInButton, "Sign in");
    }
}