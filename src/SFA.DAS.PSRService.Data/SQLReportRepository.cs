using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;

namespace SFA.DAS.PSRService.Data
{
    public class SQLReportRepository : IReportRepository
    {
        private readonly string _connectionString;

        public SQLReportRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ReportDto Get(string period, string employerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var report = connection.QuerySingleOrDefault<ReportDto>("select * from Report where EmployerID = @employerId and ReportingPeriod = @period",
                    new {employerId, period});
                return report;
            }
        }

        public ReportDto Get(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QuerySingleOrDefault<ReportDto>("select * from Report where Id = @id", new {id});
            }
        }

        public IList<ReportDto> GetSubmitted(string employerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var reportData = connection.Query<ReportDto>("select * from dbo.Report where EmployerID = @employerId and Submitted = 1",
                    new {employerId});
                return reportData.ToList();
            }
        }

        public void Create(ReportDto report)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(@"
                    INSERT INTO [dbo].[Report] ([Id],[EmployerId],[ReportingPeriod],[ReportingData],[Submitted],[AuditWindowStartUtc],[UpdatedUtc],[UpdatedBy])
                                        VALUES (@Id, @EmployerId, @ReportingPeriod, @ReportingData, @Submitted, @AuditWindowStartUtc, @UpdatedUtc, @UpdatedBy)",
                    new {report.Id, report.EmployerId, report.ReportingData, report.ReportingPeriod, report.Submitted, report.AuditWindowStartUtc, report.UpdatedUtc, report.UpdatedBy});
            }
        }

        public void Update(ReportDto reportDto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(@"
                    UPDATE [dbo].[Report]
                       SET [ReportingData] = @ReportingData
                          ,[Submitted] = @Submitted
                          ,[AuditWindowStartUtc] = @AuditWindowStartUtc
                          ,[UpdatedUtc] = @UpdatedUtc
                          ,[UpdatedBy] = @UpdatedBy
                     WHERE Id = @Id",
                    new {reportDto.ReportingData, reportDto.Submitted, reportDto.AuditWindowStartUtc, reportDto.UpdatedUtc, reportDto.UpdatedBy, reportDto.Id});
            }
        }

        public void SaveAuditRecord(AuditRecordDto auditRecordDto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(@"
                INSERT INTO [dbo].[AuditHistory]
                    ([UpdatedUtc]
                    ,[ReportingData]
                    ,[UpdatedBy]
                    ,[ReportId])
                VALUES
                    (@UpdatedUtc
                    ,@ReportingData
                    ,@UpdatedBy
                    ,@ReportId)",
                    new {auditRecordDto.UpdatedUtc, auditRecordDto.ReportingData, auditRecordDto.UpdatedBy, auditRecordDto.ReportId});
            }
        }

        public IReadOnlyList<AuditRecordDto> GetAuditRecordsMostRecentFirst(Guid reportId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<AuditRecordDto>("select * from dbo.AuditHistory where ReportId = @reportId order by UpdatedUtc desc", new {reportId}).ToList();
            }        
        }
    }
}