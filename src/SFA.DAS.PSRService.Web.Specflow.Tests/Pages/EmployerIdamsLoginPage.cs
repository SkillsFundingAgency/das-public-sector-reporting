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

        private readonly By _selectPageForm = By.Id("selectOption");
        private readonly By _submitPageFormButton = By.Id("SubmitSelectOptionForm");

        internal void SelectPreviouslySubmittedReports()
        {
            FormCompletionHelper.SelectRadioOptionByForAttribute(_selectPageForm, "home-action-list");
           
        }
        private readonly By _emailAddress = By.Name("EmailAddress");
        private readonly By _password = By.Name("Password");
        private readonly By _signInButton = By.Id("button-signin");

        internal void Login(string emailAddress, string password)
        {
            FormCompletionHelper.EnterText(_emailAddress, emailAddress);
            FormCompletionHelper.EnterText(_password, password);

            FormCompletionHelper.ClickElement(_signInButton);

          // var loading = new EmployerIdamsLodingPage(WebDriver);

          //  loading


          //  return new PsrsHomepage(WebDriver);
        }

       
    }
}