using SFA.DAS.PSRService.Application.Domain;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_I_Have_Created_A_Report.And_I_Have_Updated_The_Report;

[ExcludeFromCodeCoverage]
public abstract class And_I_Have_Updated_The_Report_Data : Given_I_Have_Created_A_Report
{
    protected ReportDto UpdatedReport { get; set; }

    protected override async Task Given()
    {
        await base.Given();

        UpdatedReport = new ReportDto
        {
            Id = CreatedReport.Id,
            EmployerId = CreatedReport.EmployerId,
            ReportingPeriod = CreatedReport.ReportingPeriod,
            Submitted = CreatedReport.Submitted,
            UpdatedUtc = RepositoryTestHelper.TrimDateTime(DateTime.UtcNow.AddSeconds(-5)),
            AuditWindowStartUtc = RepositoryTestHelper.TrimDateTime(DateTime.UtcNow.AddSeconds(-10)),
            ReportingData = "Wikileaks",
            UpdatedBy = "Igor"
        };

        await Sut.Update(UpdatedReport);
    }
}