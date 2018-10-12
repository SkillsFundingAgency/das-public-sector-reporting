using System;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public class EmployerIdamsLoginPage : BasePage
    {
        private static String PAGE_TITLE = "Sign in";
        public EmployerIdamsLoginPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public override void Navigate()
        {
            throw new NotImplementedException("Page should never be called directly");
        }
        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(this.GetPageHeading(), PAGE_TITLE);
        }

        internal void SelectPreviouslySubmittedReports()
        {
            FormCompletionHelper.SelectRadioOptionById( "home-action-list");
        }

        private readonly By _emailAddress = By.Name("EmailAddress");
        private readonly By _password = By.Name("Password");
        private readonly By _signInButton = By.Id("button-signin");

        internal void Login(TestUser user)
        {
            FormCompletionHelper.EnterText(_emailAddress, user.Email);
            FormCompletionHelper.EnterText(_password, user.Password);

            FormCompletionHelper.ClickElement(_signInButton);

          // var loading = new EmployerIdamsLodingPage(WebDriver);

          //  loading


          //  return new PsrsHomepage(WebDriver);
        }
    }
}