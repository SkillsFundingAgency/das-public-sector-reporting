using System;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport
{
    public abstract class BasePage
    {
        protected IWebDriver WebDriver;
        private By _pageHeading = By.CssSelector("h1");

        public BasePage(IWebDriver webDriver)
        {
            this.WebDriver = webDriver;
            SeleniumExtras.PageObjects.PageFactory.InitElements(webDriver, this);
        }

        public abstract Boolean Verify();

        public abstract void Navigate();

        protected String GetPageHeading()
        {
            return WebDriver.FindElement(_pageHeading).Text;
        }

        protected string GetPageUrl(string page)
        {
          return Configurator.GetConfiguratorInstance().GetBaseUrl() +
                            Configurator.GetConfiguratorInstance().GetEmployerId() +
                            page;
        }
    }
}