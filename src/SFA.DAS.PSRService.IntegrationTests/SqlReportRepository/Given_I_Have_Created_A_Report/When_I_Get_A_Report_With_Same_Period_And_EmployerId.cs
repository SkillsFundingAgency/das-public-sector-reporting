using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_I_Have_Created_A_Report
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class When_I_Get_A_Report_With_Same_Period_And_EmployerId
        : Given_I_Have_Created_A_Report
    {
        private ReportDto retrievedReport;

        protected override void When()
        {
            retrievedReport
                =
                SUT
                    .Get(
                        CreatedReport.ReportingPeriod
                        , CreatedReport.EmployerId);
        }

        [Test]
        public void Then_Retrieved_Report_Is_Equivalent_To_Created_Report()
        {
            RepositoryTestHelper
                .AssertReportsAreEquivalent(
                    CreatedReport
                    , retrievedReport);
        }
    }
}