using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions.Shared
{
    [Binding]
    public sealed class AuthorizationSharedSteps : BaseTest
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [Given(@"Full access is granted")]
        public void SuperUserAcccess()
        {
            pageFactory.Homepage.Navigate();
            
            if (pageFactory.Homepage.IsSignInDisplayed)
            {
                var user = Configurator.GetConfiguratorInstance().GetSuperUser();
                ScenarioContext.Current.Set(user, ContextKeys.CurrentUser);

                pageFactory.IdamsLogin.Login(user);

                pageFactory.Homepage.Verify();
            }
        }

        [Given(@"Edit access is granted")]
        public void EditAccess()
        {
            pageFactory.Homepage.Navigate();

            if (pageFactory.Homepage.IsSignInDisplayed)
            {
                var user = Configurator.GetConfiguratorInstance().GetEditUser();
                ScenarioContext.Current.Set(user, ContextKeys.CurrentUser);

                pageFactory.IdamsLogin.Login(user);

                pageFactory.Homepage.Verify();
            }
        }

        [Given(@"View access is granted")]
        public void ViewAccess()
        {
            pageFactory.Homepage.Navigate();

            if (pageFactory.Homepage.IsSignInDisplayed)
            {
                var user = Configurator.GetConfiguratorInstance().GetViewUser();
                ScenarioContext.Current.Set(user, ContextKeys.CurrentUser);

                pageFactory.IdamsLogin.Login(user);

                pageFactory.Homepage.Verify();
            }
        }
    }
}
