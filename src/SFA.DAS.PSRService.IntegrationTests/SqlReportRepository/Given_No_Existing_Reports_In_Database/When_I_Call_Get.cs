using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_No_Existing_Reports_In_Database;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class When_I_Call_Get
    : Given_No_Existing_Reports_In_Database
{
    private ReportDto retrievedReport;

    protected override void When()
    {
        retrievedReport
            =
            SUT
                .Get(
                    "SomeReportingPeriod"
                    , "SomeEmployerId");
    }

    [Test]
    public void Then_Retrieved_Report_Is_Null()
    {
        Assert
            .IsNull(
                retrievedReport);
    }
}