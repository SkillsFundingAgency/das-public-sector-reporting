using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using SFA.DAS.GovUK.Auth.Employer;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Filters;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests;

public class WhenAddingServicesToTheContainer
{
    [TestCase(typeof(ContentPolicyReportController))]
    [TestCase(typeof(HomeController))]
    [TestCase(typeof(QuestionController))]
    [TestCase(typeof(ReportController))]
    [TestCase(typeof(ServiceController))]
    public void Then_The_Dependencies_Are_Correctly_Resolved_For_Controllers(Type toResolve)
    {
        RunTestForType(toResolve);
    }
    
    [TestCase(typeof(GoogleAnalyticsFilter))]
    [TestCase(typeof(ZenDeskApiFilter))]
    public void Then_The_Dependencies_Are_Correctly_Resolved_For_Filters(Type toResolve)
    {
        RunTestForType(toResolve);
    }

    [TestCase(typeof(IEmployerAccountService))]
    [TestCase(typeof(IReportService))]
    [TestCase(typeof(IPeriodService))]
    [TestCase(typeof(IDateTimeService))]
    [TestCase(typeof(IGovAuthEmployerAccountService))]
    public void Then_The_Dependencies_Are_Correctly_Resolved_For_Services(Type toResolve)
    {
        RunTestForType(toResolve);
    }
    
    private static void RunTestForType(Type toResolve)
    {
        var mockHostEnvironment = new Mock<IHostEnvironment>();
        mockHostEnvironment.Setup(x => x.EnvironmentName).Returns("Test");
        mockHostEnvironment.Setup(x => x.ContentRootFileProvider).Returns(Mock.Of<IFileProvider>());

        var startup = new Startup(GenerateStubConfiguration(), mockHostEnvironment.Object, buildConfig: false);
        var serviceCollection = new ServiceCollection();
        startup.ConfigureServices(serviceCollection);
        
        serviceCollection.AddTransient<ContentPolicyReportController>();
        serviceCollection.AddTransient<HomeController>();
        serviceCollection.AddTransient<QuestionController>();
        serviceCollection.AddTransient<ReportController>();
        serviceCollection.AddTransient<ServiceController>();

        var provider = serviceCollection.BuildServiceProvider();

        var type = provider.GetService(toResolve);
        type.Should().NotBeNull();
    }

    private static ConfigurationRoot GenerateStubConfiguration()
    {
        var configSource = new MemoryConfigurationSource
        {
            InitialData = new List<KeyValuePair<string, string>>
            {
                new("SFA.DAS.Encoding", "{\"Encodings\": [{\"EncodingType\": \"AccountId\",\"Salt\": \"and vinegar\",\"MinHashLength\": 32,\"Alphabet\": \"46789BCDFGHJKLMNPRSTVWXY\"}]}"),
                new("Environment", "test"),
                new("EnvironmentName", "test"),
                new("ResourceEnvironmentName", "test"),
                new("AccountsApi:ApiBaseUrl", "https://test.test"),
                new("AccountsApi:IdentifierUri", "https://test.test"),
                new("OuterApiConfiguration:Key", "ABC123"),
                new("OuterApiConfiguration:BaseUrl", "https://test.test"),
                new("Identity:ClientID", "PSRTEST"),
                new("DataProtectionKeysDatabase", "TESTDATABASE"),
                new("SessionStore:Type", "Redis"),
                new("SessionStore:ConnectionString", "TEST"),
                new("GovUkOidcConfiguration:BaseUrl", "https://test.test"),
                new("GovUkOidcConfiguration:KeyVaultIdentifier", "DSSJGHSD"),
            }
        };

        var provider = new MemoryConfigurationProvider(configSource);

        return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
    }
}