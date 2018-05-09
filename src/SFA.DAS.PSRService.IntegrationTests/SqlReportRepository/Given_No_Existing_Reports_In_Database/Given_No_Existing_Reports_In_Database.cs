using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Data;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_No_Existing_Reports_In_Database
{
    [ExcludeFromCodeCoverage]
    public class Given_No_Existing_Reports_In_Database : GivenWhenThen<IReportRepository>
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