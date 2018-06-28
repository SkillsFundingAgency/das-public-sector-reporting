using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_AuditHistory_For_Two_Reports
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public sealed  class When_I_Delete_History_For_One_Report
    :Given_AuditHistory_For_Two_Reports
    {
        protected override void When()
        {
            SUT
                .DeleteHistory(
                    RepositoryTestHelper.ReportTwoId);
        }


        [Test]
        public void Then_All_History_For_That_Report_Is_Removed()
        {
            RepositoryTestHelper
                .GetAllAuditHistory()
                .Should()
                .NotContain(
                    record => record.ReportId == RepositoryTestHelper.ReportTwoId);
        }

        [Test]
        public void Then_All_History_For_Other_Report_Remains_Intact()
        {
            RepositoryTestHelper
                .GetAllAuditHistory()
                .Should()
                .OnlyContain(
                    record => record.ReportId == RepositoryTestHelper.ReportOneId);

            RepositoryTestHelper
                .GetAllAuditHistory()
                .Should()
                .HaveCount(
                    5);
        }
    }
}