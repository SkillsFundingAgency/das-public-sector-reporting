using System.Collections.Generic;
using SFA.DAS.PSRService.Application.Domain;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_One_Submitted_Report.And_One_NonSubmitted_Report;

[ExcludeFromCodeCoverage]
[TestFixture]
public class When_I_Call_GetSubmitted_For_EmployerId : And_One_NonSubmitted_Report
{
    private List<ReportDto> _submittedReports;

    protected override async Task When()
    {
        _submittedReports = await Sut.GetSubmitted(EmployerId);
    }

    [Test]
    public void Then_Only_One_Report_Is_Returned()
    {
        _submittedReports.Count.Should().Be(1);
    }

    [Test]
    public void Then_Retrieved_Report_Is_Equivalent_To_SubmittedReport()
    {
        RepositoryTestHelper.AssertReportsAreEquivalent(SubmittedReport, _submittedReports.Single());
    }
}