using System.Collections.Generic;
using SFA.DAS.PSRService.Application.Domain;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_No_Existing_Reports_In_Database;

[ExcludeFromCodeCoverage]
public sealed class When_I_Call_GetSubmitted
    : Given_No_Existing_Reports_In_Database
{
    private List<ReportDto> _retrievedSubmittedReports;

    protected override async Task When()
    {
        _retrievedSubmittedReports = await Sut.GetSubmitted("DummyEmployerId");
    }

    [Test]
    public void Then_An_Empty_Collection_Is_Returned()
    {
        _retrievedSubmittedReports.Should().NotBeNull();
        _retrievedSubmittedReports.Should().BeEmpty();
    }
}