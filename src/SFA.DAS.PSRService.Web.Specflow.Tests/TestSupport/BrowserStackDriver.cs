using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrowserStack;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport
{
    class BrowserStackDriver
    {
        private IWebDriver driver;
        private Local browserStackLocal;
        private string profile;
        private string environment;
        private ScenarioContext context;

        public BrowserStackDriver(ScenarioContext context)
        {
            this.context = context;
        }

        public IWebDriver Init(string profile, string environment)
        {
            NameValueCollection caps = ConfigurationManager.GetSection("capabilities/" + profile) as NameValueCollection;
            NameValueCollection settings = ConfigurationManager.GetSection("environments/" + environment) as NameValueCollection;

            DesiredCapabilities capability = new DesiredCapabilities();

            foreach (string key in caps.AllKeys)
            {
                capability.SetCapability(key, caps[key]);
            }

            foreach (string key in settings.AllKeys)
            {
                capability.SetCapability(key, settings[key]);
            }

            String username = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");
            if (username == null)
            {
                username = ConfigurationManager.AppSettings.Get("BrowserStack.User");
            }

            String accesskey = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");
            if (accesskey == null)
            {
                accesskey = ConfigurationManager.AppSettings.Get("BrowserStack.Key");
            }

            capability.SetCapability("browserstack.user", username);
            capability.SetCapability("browserstack.key", accesskey);
            capability.SetCapability("project", "das-psrs");
            capability.SetCapability("build", "build x.x");

            File.AppendAllText("C:\\Users\\Matt\\Desktop\\sf.log", "Starting local");

            if (capability.GetCapability("browserstack.local") != null && capability.GetCapability("browserstack.local").ToString() == "true")
            {
                browserStackLocal = new Local();
                List<KeyValuePair<string, string>> bsLocalArgs = new List<KeyValuePair<string, string>>() {
          new KeyValuePair<string, string>("key", accesskey)
        };
                browserStackLocal.start(bsLocalArgs);
            }

            File.AppendAllText("C:\\Users\\Matt\\Desktop\\sf.log", "Starting driver");
            driver = new RemoteWebDriver(new Uri("http://" + ConfigurationManager.AppSettings.Get("BrowserStack.Server") + "/wd/hub/"), capability);
            return driver;
        }

        public void Cleanup()
        {
            driver.Quit();
            if (browserStackLocal != null)
            {
                browserStackLocal.stop();
            }
        }
    }
}
