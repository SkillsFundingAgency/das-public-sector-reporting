using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.Pages;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
    [Binding]
    public sealed class AuthorizationSharedSteps : BaseTest
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [Given(@"Full access is granted")]
        public void SuperUserAcccess()
        {
            webDriver.Url = Configurator.GetConfiguratorInstance().GetBaseUrl();
            
            if (webDriver.FindElement(By.CssSelector("h1")).Text == "Sign in")
            {
                var loginPage = new EmployerIdamsLoginPage(webDriver);

                loginPage.LoginAsSuperUser(Configurator.GetConfiguratorInstance().GetSuperUser(), Configurator.GetConfiguratorInstance().GetSuperUserPassword());

               // var loading = new EmployerIdamsLodingPage(webDriver);

                var homepage = new PsrsHomepage(webDriver);
            }

        }

        [Given(@"Edit access is granted")]
        public void EditAccess()
        {
            webDriver.Url = Configurator.GetConfiguratorInstance().GetBaseUrl();

            if (webDriver.FindElement(By.CssSelector("h1")).Text == "Sign in")
            {
                var loginPage = new EmployerIdamsLoginPage(webDriver);

                loginPage.LoginAsSuperUser(Configurator.GetConfiguratorInstance().GetEditUser(), Configurator.GetConfiguratorInstance().GetEditUserPassword());

                // var loading = new EmployerIdamsLodingPage(webDriver);

                var homepage = new PsrsHomepage(webDriver);
            }

        }

        [Given(@"View access is granted")]
        public void ViewAccess()
        {
            webDriver.Url = Configurator.GetConfiguratorInstance().GetBaseUrl();

            if (webDriver.FindElement(By.CssSelector("h1")).Text == "Sign in")
            {
                var loginPage = new EmployerIdamsLoginPage(webDriver);

                loginPage.LoginAsSuperUser(Configurator.GetConfiguratorInstance().GetViewUser(), Configurator.GetConfiguratorInstance().GetViewUserPassword());

                // var loading = new EmployerIdamsLodingPage(webDriver);

                var homepage = new PsrsHomepage(webDriver);
            }

        }

    }
}
