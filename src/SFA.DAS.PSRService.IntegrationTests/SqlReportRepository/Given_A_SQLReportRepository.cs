using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
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

            SUT = new SQLReportRepository(RepositoryTestHelper.ConnectionString);
        }

        [TearDown]
        public void ClearDatabase()
        {
            RepositoryTestHelper
                .ClearData();
        }
    }
}