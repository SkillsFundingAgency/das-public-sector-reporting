using SFA.DAS.PSRService.Application.Interfaces;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository;

[ExcludeFromCodeCoverage]
public class Given_A_SQLReportRepository : GivenWhenThen<IReportRepository>
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