using System;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public class EmployerIdamsLodingPage : BasePage
    {
        private static String PAGE_TITLE = "Loading...";

        public EmployerIdamsLodingPage(IWebDriver webDriver) : base(webDriver)
        {
            SelfVerify();
        }

        protected override bool SelfVerify()
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

        internal PsrsHomepage LoginAsSuperUser(string emailAddress, string password)
        {
            FormCompletionHelper.EnterText(_emailAddress, emailAddress);
            FormCompletionHelper.EnterText(_password, password);

            FormCompletionHelper.ClickElement(_signInButton);



            return new PsrsHomepage(WebDriver);
        }

       
    }
}