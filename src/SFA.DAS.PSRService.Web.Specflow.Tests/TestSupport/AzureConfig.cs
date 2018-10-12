using System;
using System.Configuration;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport
{
    public  static class AzureConfig
    {
        static readonly Lazy<TestConfiguration> _config = new Lazy<TestConfiguration>(loadConfig);

        private static TestConfiguration loadConfig()
        {
            var configurationRepository = new AzureTableStorageConfigurationRepository(ConfigurationManager.AppSettings["ConfigurationStorageConnectionString"]);

            var configurationService =
                new ConfigurationService(
                    configurationRepository,
                    new ConfigurationOptions(ConfigurationManager.AppSettings["ServiceName"],
                        ConfigurationManager.AppSettings["EnvironmentName"],
                        ConfigurationManager.AppSettings["Version"]));

            return configurationService.Get<TestConfiguration>();
        }

        public static TestConfiguration GetConfig()
        {
            return _config.Value;
        }
    }
}