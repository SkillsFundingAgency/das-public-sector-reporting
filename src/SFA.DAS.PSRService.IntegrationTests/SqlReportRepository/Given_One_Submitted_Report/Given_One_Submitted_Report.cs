using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_One_Submitted_Report;

[ExcludeFromCodeCoverage]
public abstract class Given_One_Submitted_Report
    : GivenWhenThen<IReportRepository>
{
    protected ReportDto SubmittedReport;
    protected readonly string EmployerId = "TestEmployerID";

    protected override async Task Given()
    {
        RepositoryTestHelper.ClearData();

        Sut = new Data.SqlReportRepository(new SqlConnection(RepositoryTestHelper.ConnectionString));

        SubmittedReport = new ReportDto
        {
            Id = RepositoryTestHelper.ReportOneId,
            EmployerId = EmployerId,
            ReportingData = "Some dumb piece of json",
            ReportingPeriod = "2222",
            Submitted = true
        };

        await Sut.Create(SubmittedReport);
    }

    [TearDown]
    public void ClearDatabase()
    {
        RepositoryTestHelper.ClearData();
    }
}