using System.Configuration;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport
{
    public class Configurator
    {
        private static Configurator _configuratorInstance;

        private readonly string _browser;
        private readonly bool _runBrowserInHeadlessMode;
        private readonly string _baseUrl;
        private readonly string _baseGovUkUrl;
        private readonly string _browserStackBrowser;
        private readonly string _employerId;

        private readonly TestUser _superUser;

        private readonly TestUser _editUser;

        private readonly TestUser _viewUser;

        private Configurator()
        {
            _browser = ConfigurationManager.AppSettings["Browser"];
            bool.TryParse(ConfigurationManager.AppSettings["RunBrowserInHeadlessMode"], out _runBrowserInHeadlessMode);
            _baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            _baseGovUkUrl = ConfigurationManager.AppSettings["BaseGovUkUrl"];
            _browserStackBrowser = ConfigurationManager.AppSettings["BrowserStack.Browser"];
            _employerId = ConfigurationManager.AppSettings["EmployerId"];

            _viewUser = new TestUser
            {
                Id = "a78ef43b-e370-43e1-86db-6556a8268990",
                Email = "Sender1@gmail.com",
                Password = "Sender1@gm",
                DisplayName = "Sender 1"
            };

            _editUser = new TestUser
            {
                Id = "0b7a9411-ca0b-401e-9008-4aa3c1f7e0c1",
                Email = "Sender2@gmail.com",
                Password = "Sender2@gm",
                DisplayName = "Sender 2"
            };
            _superUser = new TestUser
            {
                Id = "6435a0db-6dd0-439b-963a-1210215785e0",
                Email = "Sender3@gmail.com",
                Password = "Sender3@gm",
                DisplayName = "Sender 3"
            };
        }

        public static Configurator GetConfiguratorInstance()
        {
            return _configuratorInstance ?? (_configuratorInstance = new Configurator());
        }

        public string GetBrowser()
        {
            return _browser;
        }

        public bool GetRunBrowserInHeadlessMode()
        {
            return _runBrowserInHeadlessMode;
        }

        public string GetBaseGovUkUrl()
        {
            return _baseGovUkUrl;
        }

        public string GetBaseUrl()
        {
            return _baseUrl;
        }

        public string GetBrowserStackBrowser()
        {
            return _browserStackBrowser;
        }

        public string GetEmployerId()
        {
            return _employerId;
        }

        public TestUser GetSuperUser()
        {
            return _superUser;
        }

        public TestUser GetEditUser()
        {
            return _editUser;
        }

        public TestUser GetViewUser()
        {
            return _viewUser;
        }
    }
}