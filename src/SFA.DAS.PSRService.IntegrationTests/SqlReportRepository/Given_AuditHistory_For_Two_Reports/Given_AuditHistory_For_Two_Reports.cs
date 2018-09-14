using System;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Data;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_AuditHistory_For_Two_Reports
{
    [ExcludeFromCodeCoverage]
    public abstract class Given_AuditHistory_For_Two_Reports
        : Given_A_SQLReportRepository
    {
        protected override void Given()
        {
            base.Given();

            RepositoryTestHelper.ClearData();

            BuildAndSaveAuditHistoryForReportOne();
            BuildAndSaveAuditHistoryForReportTwo();
        }

        private void BuildAndSaveAuditHistoryForReportTwo()
        {
            CreateAndSaveNRecordsForReportId(3, RepositoryTestHelper.ReportTwoId);
        }

        private void BuildAndSaveAuditHistoryForReportOne()
        {
            CreateAndSaveNRecordsForReportId(5, RepositoryTestHelper.ReportOneId);
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