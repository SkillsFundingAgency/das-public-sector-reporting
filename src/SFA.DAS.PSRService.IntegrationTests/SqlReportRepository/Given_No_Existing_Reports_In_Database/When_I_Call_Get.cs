using SFA.DAS.PSRService.Application.Domain;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_No_Existing_Reports_In_Database;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class When_I_Call_Get : Given_No_Existing_Reports_In_Database
{
    private ReportDto _retrievedReport;

    protected override async Task When()
    {
        _retrievedReport = await Sut.Get("SomeReportingPeriod", "SomeEmployerId");
    }

    [Test]
    public void Then_Retrieved_Report_Is_Null()
    {
        _retrievedReport.Should().BeNull();
    }
}