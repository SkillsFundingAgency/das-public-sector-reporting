using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_I_Have_Created_A_Report
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class When_I_Get_A_Report_With_Same_Id : Given_I_Have_Created_A_Report
    {
        private ReportDto _retrievedReport;

        protected override void When()
        {
            base.When();

            _retrievedReport = SUT.Get(CreatedReport.Id);
        }

        [Test]
        public void Then_Retrieved_Report_Is_Equivalent_To_Created_Report()
        {
            RepositoryTestHelper
                .AssertReportsAreEquivalent(
                    CreatedReport
                    , _retrievedReport);
        }
    }
}