using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_I_Have_Created_A_Report.And_I_Have_Updated_The_Report
{
    [ExcludeFromCodeCoverage]
    public abstract class And_I_Have_Updated_The_Report_Data
    : Given_I_Have_Created_A_Report
    {
        protected string UpdatedReportingData = "Updated report data.";

        protected override void Given()
        {
            base.Given();

            CreatedReport
                    .ReportingData
                =
                UpdatedReportingData;

            SUT
                .Update(
                    CreatedReport);
        }
    }
}