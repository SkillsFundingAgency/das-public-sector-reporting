using System;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Data;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_AuditHistory_For_Two_Reports
{
    [ExcludeFromCodeCoverage]
    public abstract class Given_AuditHistory_For_Two_Reports
        : GivenWhenThen<IReportRepository>
    {
        protected Guid reportOneId = new Guid("CDF3F279-3AE1-45A7-B0E6-01B06621853B");
        protected Guid reportTwoId = new Guid("B5B28BD5-6B3B-460F-8576-F367483B54C1");
        protected override void Given()
        {
            RepositoryTestHelper.ClearData();

            SUT = new SQLReportRepository(RepositoryTestHelper.ConnectionString);

            BuildAndSaveAuditHistoryForReportOne();
            BuildAndSaveAuditHistoryForReportTwo();
        }

        private void BuildAndSaveAuditHistoryForReportTwo()
        {
            CreateAndSaveNRecordsForReportId(3, reportTwoId);
        }

        private void BuildAndSaveAuditHistoryForReportOne()
        {
            CreateAndSaveNRecordsForReportId(5, reportOneId);
        }

        private void CreateAndSaveNRecordsForReportId(int numberOfRecords, Guid reportId)
        {
            FirstAddReportToSatisfyForeignKeyConstraint(reportId);

            for (int count = 1; count <= numberOfRecords; count++)
            {
                SUT
                    .SaveAuditRecord(
                        new AuditRecordDto
                        {
                            ReportId = reportId,
                            ReportingData = count.ToString(),
                            UpdatedBy = "User" + count,
                            UpdatedUtc = DateTime.UtcNow
                        });
            }
        }

        private void FirstAddReportToSatisfyForeignKeyConstraint(Guid reportId)
        {
            SUT
                .Create(
                    new ReportDto
                    {
                        Id = reportId,
                        EmployerId = reportId.ToString(),
                        ReportingPeriod = "0000",
                        ReportingData = "SomethingNotNull",
                        Submitted = false
                    });
        }
    }
    }