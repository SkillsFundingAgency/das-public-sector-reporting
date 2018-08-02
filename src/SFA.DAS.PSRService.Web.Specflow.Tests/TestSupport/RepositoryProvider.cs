using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using BoDi;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using SFA.DAS.PSRService.Web.Specflow.Tests.Repository;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport
{
    [Binding]
    [ExcludeFromCodeCoverage]
    public class RepositoryProvider
    {
        private readonly IObjectContainer _container;
        private SQLReportRepository _reportRepository;

        public RepositoryProvider(IObjectContainer container)
        {
            _container = container;

            InstantiateRepository();
        }

        [BeforeScenario]
        public void AddRepositoryToContext()
        {
            _container.RegisterInstanceAs<SQLReportRepository>(_reportRepository);
        }

        private void InstantiateRepository()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[PersistenceNames.PsrsDBConnectionString]
                .ConnectionString;

            _reportRepository = new SQLReportRepository(connectionString);
        }
    }
}