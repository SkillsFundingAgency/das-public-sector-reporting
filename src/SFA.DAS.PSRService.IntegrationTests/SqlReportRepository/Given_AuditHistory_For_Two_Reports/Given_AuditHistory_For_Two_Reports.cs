using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_AuditHistory_For_Two_Reports;

[ExcludeFromCodeCoverage]
public abstract class Given_AuditHistory_For_Two_Reports : GivenWhenThen<IReportRepository>
{
    protected override async Task Given()
    {
        RepositoryTestHelper.ClearData();

        Sut = new Data.SqlReportRepository(new SqlConnection(RepositoryTestHelper.ConnectionString));

        await BuildAndSaveAuditHistoryForReportOne();
        await BuildAndSaveAuditHistoryForReportTwo();
    }

    private async Task BuildAndSaveAuditHistoryForReportTwo()
    {
        await CreateAndSaveNRecordsForReportId(3, RepositoryTestHelper.ReportTwoId);
    }

    private async Task BuildAndSaveAuditHistoryForReportOne()
    {
        await CreateAndSaveNRecordsForReportId(5, RepositoryTestHelper.ReportOneId);
    }

    private async Task CreateAndSaveNRecordsForReportId(int numberOfRecords, Guid reportId)
    {
        await FirstAddReportToSatisfyForeignKeyConstraint(reportId);

        for (var count = 1; count <= numberOfRecords; count++)
        {
            await Sut.SaveAuditRecord(new AuditRecordDto
            {
                ReportId = reportId,
                ReportingData = count.ToString(),
                UpdatedBy = "User" + count,
                UpdatedUtc = DateTime.UtcNow
            });
        }
    }

    private async Task FirstAddReportToSatisfyForeignKeyConstraint(Guid reportId)
    {
        await Sut.Create(new ReportDto
        {
            Id = reportId,
            EmployerId = reportId.ToString(),
            ReportingPeriod = "0000",
            ReportingData = "SomethingNotNull",
            Submitted = false
        });
    }
}