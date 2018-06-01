using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.Pages;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
    [Binding]
    public sealed class GivenFullAccess : BaseTest
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [Given(@"Full access is granted")]
        public void BeforeFeature()
        {
            webDriver.Url = Configurator.GetConfiguratorInstance().GetBaseUrl();
            
            if (webDriver.FindElement(By.CssSelector("h1")).Text == "Sign in")
            {
                var loginPage = new EmployerIdamsLoginPage(webDriver);

                loginPage.LoginAsSuperUser("matt.derry@digital.education.gov.uk", "Service123");

               // var loading = new EmployerIdamsLodingPage(webDriver);

                var homepage = new PsrsHomepage(webDriver);
            }

        }

    }
}
