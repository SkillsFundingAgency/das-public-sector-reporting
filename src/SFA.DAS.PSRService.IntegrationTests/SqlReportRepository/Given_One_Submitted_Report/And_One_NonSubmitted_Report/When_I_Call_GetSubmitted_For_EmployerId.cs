using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_One_Submitted_Report.And_One_NonSubmitted_Report
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class When_I_Call_GetSubmitted_For_EmployerId
    : And_One_NonSubmitted_Report
    {
        private IList<ReportDto> submittedReports;

        protected override void When()
        {
            base.When();

            submittedReports
                =
                SUT
                    .GetSubmitted(
                        EmployerId);
        }

        [Test]
        public void Then_Only_One_Report_Is_Returned()
        {
            Assert
                .AreEqual(
                    1
                    , submittedReports.Count);
        }

        [Test]
        public void Then_Retrieved_Report_Is_Equivalent_To_SubmittedReport()
        {
            RepositoryTestHelper
                .AssertReportsAreEquivalent(
                    SubmittedReport
                    , submittedReports.Single());
        }
    }
}