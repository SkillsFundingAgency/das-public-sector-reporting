using System.Configuration;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport
{
    public class Configurator
    {
        private static Configurator _configuratorInstance = null;

        private readonly string _browser;
        private readonly bool _runBrowserInHeadlessMode;
        private readonly string _baseUrl;
        private readonly string _baseGovUkUrl;
        private readonly string _browserStackBrowser;
        private readonly string _employerId;

        private readonly string _superUser;
        private readonly string _superUserPassword;
        private readonly string _editUser;
        private readonly string _editUserPassword;
        private readonly string _viewUser;
        private readonly string _viewUserPassword;


        private Configurator()
        {
            _browser = ConfigurationManager.AppSettings["Browser"];
            bool.TryParse(ConfigurationManager.AppSettings["RunBrowserInHeadlessMode"], out _runBrowserInHeadlessMode);
            _baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            _baseGovUkUrl = ConfigurationManager.AppSettings["BaseGovUkUrl"];
            _browserStackBrowser = ConfigurationManager.AppSettings["BrowserStack.Browser"];
            _employerId = ConfigurationManager.AppSettings["EmployerId"];

            _viewUser = "Sender1@gmail.com";
            _viewUserPassword = "Sender1@gm";
            _editUser = "Sender2@gmail.com";
            _editUserPassword = "Sender2@gm";
            _superUser = "Sender3@gmail.com";
            _superUserPassword = "Sender3@gm";
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
        public string GetSuperUser()
        {
            return _superUser;
        }
        public string GetSuperUserPassword()
        {
            return _superUserPassword;
        }
        public string GetEditUser()
        {
            return _editUser;
        }
        public string GetEditUserPassword()
        {
            return _editUserPassword;
        }
        public string GetViewUser()
        {
            return _viewUser;
        }

        public string GetViewUserPassword()
        {
            return _viewUserPassword;
        }
    }
}