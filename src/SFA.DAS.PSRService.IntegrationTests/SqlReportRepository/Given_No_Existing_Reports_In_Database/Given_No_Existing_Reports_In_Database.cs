using SFA.DAS.PSRService.Application.Interfaces;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_No_Existing_Reports_In_Database;

[ExcludeFromCodeCoverage]
public class Given_No_Existing_Reports_In_Database : GivenWhenThen<IReportRepository>
{
    protected override Task Given()
    {
        RepositoryTestHelper.ClearData();

        Sut = new Data.SqlReportRepository(new SqlConnection(RepositoryTestHelper.ConnectionString));

        return Task.CompletedTask;
    }

    [TearDown]
    public void ClearDatabase()
    {
        RepositoryTestHelper.ClearData();
    }
}