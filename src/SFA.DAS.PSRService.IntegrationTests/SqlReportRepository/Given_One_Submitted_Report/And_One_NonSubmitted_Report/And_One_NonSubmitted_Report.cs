using SFA.DAS.PSRService.Application.Domain;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_One_Submitted_Report.And_One_NonSubmitted_Report;

[ExcludeFromCodeCoverage]
public abstract class And_One_NonSubmitted_Report : Given_One_Submitted_Report
{
    private ReportDto _nonsubmittedReport;

    protected override  async Task Given()
    {
        await base.Given();

        _nonsubmittedReport = new ReportDto
        {
            Id = RepositoryTestHelper.ReportTwoId,
            EmployerId = EmployerId,
            ReportingData = "Some junk piece of json",
            ReportingPeriod = "Noo!",
            Submitted = false
        };

        await Sut.Create(_nonsubmittedReport);
    }
}