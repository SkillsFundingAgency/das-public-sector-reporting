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
            pageFactory.Homepage.Navigate();
            
            if (webDriver.FindElement(By.CssSelector("h1")).Text == "Sign in")
            {
                pageFactory.IdamsLogin.Login(Configurator.GetConfiguratorInstance().GetSuperUser(), Configurator.GetConfiguratorInstance().GetSuperUserPassword());

                pageFactory.Homepage.Verify();
            }

        }

        [Given(@"Edit access is granted")]
        public void EditAccess()
        {
            pageFactory.Homepage.Navigate();

            if (webDriver.FindElement(By.CssSelector("h1")).Text == "Sign in")
            {
                pageFactory.IdamsLogin.Login(
                                            Configurator.GetConfiguratorInstance().GetEditUser()
                                            , Configurator.GetConfiguratorInstance().GetEditUserPassword());

                pageFactory.Homepage.Verify();
            }
        }

        [Given(@"View access is granted")]
        public void ViewAccess()
        {
            pageFactory.Homepage.Navigate();

            if (webDriver.FindElement(By.CssSelector("h1")).Text == "Sign in")
            {
                pageFactory.IdamsLogin.Login(Configurator.GetConfiguratorInstance().GetViewUser(), Configurator.GetConfiguratorInstance().GetViewUserPassword());

                pageFactory.Homepage.Verify();
            }
            
        }

    }
}
