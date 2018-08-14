using System.Configuration;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport
{
    public static class AzureConfig
    {
        public static TestConfiguration Getconfig()
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
    }
}