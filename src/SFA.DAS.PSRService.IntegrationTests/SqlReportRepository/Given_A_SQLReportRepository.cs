using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;
using SFA.DAS.NServiceBus;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Data;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository
{
    [ExcludeFromCodeCoverage]
    public class Given_A_SQLReportRepository : GivenWhenThen<IReportRepository>
    {
        protected override void Given()
        {
            RepositoryTestHelper
                .ClearData();

            var mockUnitOfWorkContext = new Mock<IUnitOfWorkContext>();

            var connection = new SqlConnection(RepositoryTestHelper.ConnectionString);

            mockUnitOfWorkContext
                .Setup(
                    m => m.Get<DbConnection>())
                .Returns(connection);

            SUT = new SQLReportRepository(mockUnitOfWorkContext.Object);
        }

        [TearDown]
        public void ClearDatabase()
        {
            RepositoryTestHelper
                .ClearData();
        }
    }
}