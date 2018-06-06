﻿using System.Configuration;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport
{
    public class Configurator
    {
        private static Configurator _configuratorInstance = null;

        private readonly string _browser;
        private readonly string _baseUrl;
        private readonly string _baseGovUkUrl;
        private readonly string _browserStackBrowser;
        private readonly string _accountId;


        private Configurator()
        {
            _browser = ConfigurationManager.AppSettings["Browser"];
            _baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            _baseGovUkUrl = ConfigurationManager.AppSettings["BaseGovUkUrl"];
            _browserStackBrowser = ConfigurationManager.AppSettings["BrowserStack.Browser"];
            _accountId = ConfigurationManager.AppSettings["AccountId"];
        }

        public static Configurator GetConfiguratorInstance()
        {
            return _configuratorInstance ?? (_configuratorInstance = new Configurator());
        }

        public string GetBrowser()
        {
            return _browser;
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

        public string GetAccountId()
        {
            return _accountId;
        }
    }
}