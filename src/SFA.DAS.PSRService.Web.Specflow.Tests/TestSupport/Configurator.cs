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

        private Configurator()
        {
            _browser = ConfigurationManager.AppSettings["Browser"];
            bool.TryParse(ConfigurationManager.AppSettings["RunBrowserInHeadlessMode"], out _runBrowserInHeadlessMode);
            _baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            _baseGovUkUrl = ConfigurationManager.AppSettings["BaseGovUkUrl"];
            _browserStackBrowser = ConfigurationManager.AppSettings["BrowserStack.Browser"];
            _employerId = ConfigurationManager.AppSettings["EmployerId"];

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
            return AzureConfig.GetConfig().SubmitAccessUser;
        }

        public TestUser GetEditUser()
        {
            return AzureConfig.GetConfig().EditAccessUser;
        }

        public TestUser GetViewUser()
        {
            return AzureConfig.GetConfig().ViewAccessUser;
        }
    }
}