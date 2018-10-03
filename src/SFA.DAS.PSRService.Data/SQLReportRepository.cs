using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Dapper;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;

namespace SFA.DAS.PSRService.Data
{
    public class SQLReportRepository : IReportRepository
    {
        private readonly Func<DbConnection> _connectionFactory;
        private readonly Func<DbTransaction> _transactionFactory;

        public SQLReportRepository(
            Func<DbConnection> connectionFactory,
            Func<DbTransaction> transactionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _transactionFactory = transactionFactory ?? throw new ArgumentNullException(nameof(transactionFactory));
        }

        public ReportDto Get(string period, string employerId)
        {
            var report = _connectionFactory().QuerySingleOrDefault<ReportDto>("select * from Report where EmployerID = @employerId and ReportingPeriod = @period",
                         new {employerId, period}, _transactionFactory());

            return report;
        }

        public ReportDto Get(Guid id)
        {
            return _connectionFactory().QuerySingleOrDefault<ReportDto>("select * from Report where Id = @id", new {id}, _transactionFactory());
        }

        public IList<ReportDto> GetSubmitted(string employerId)
        {
            var reportData = _connectionFactory().Query<ReportDto>(
                "select * from dbo.Report where EmployerID = @employerId and Submitted = 1",
                new {employerId}, _transactionFactory());

            return reportData.ToList();
        }

        public void Create(ReportDto report)
        {
            _connectionFactory().Execute(@"
                    INSERT INTO [dbo].[Report] ([Id],[EmployerId],[ReportingPeriod],[ReportingData],[Submitted],[AuditWindowStartUtc],[UpdatedUtc],[UpdatedBy])
                                        VALUES (@Id, @EmployerId, @ReportingPeriod, @ReportingData, @Submitted, @AuditWindowStartUtc, @UpdatedUtc, @UpdatedBy)",
                    new {report.Id, report.EmployerId, report.ReportingData, report.ReportingPeriod, report.Submitted, report.AuditWindowStartUtc, report.UpdatedUtc, report.UpdatedBy}, _transactionFactory());
        }

        public void Update(ReportDto reportDto)
        {
            _connectionFactory().Execute(@"
                    UPDATE [dbo].[Report]
                       SET [ReportingData] = @ReportingData
                          ,[Submitted] = @Submitted
                          ,[AuditWindowStartUtc] = @AuditWindowStartUtc
                          ,[UpdatedUtc] = @UpdatedUtc
                          ,[UpdatedBy] = @UpdatedBy
                     WHERE Id = @Id",
                new
                {
                    reportDto.ReportingData,
                    reportDto.Submitted,
                    reportDto.AuditWindowStartUtc,
                    reportDto.UpdatedUtc,
                    reportDto.UpdatedBy,
                    reportDto.Id
                }, _transactionFactory());
        }

        public void SaveAuditRecord(AuditRecordDto auditRecordDto)
        {
            _connectionFactory().Execute(@"
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
                    new {auditRecordDto.UpdatedUtc, auditRecordDto.ReportingData, auditRecordDto.UpdatedBy, auditRecordDto.ReportId}, _transactionFactory());
        }

        public IReadOnlyList<AuditRecordDto> GetAuditRecordsMostRecentFirst(Guid reportId)
        {
            return _connectionFactory().Query<AuditRecordDto>("select * from dbo.AuditHistory where ReportId = @reportId order by UpdatedUtc desc", new {reportId}, _transactionFactory()).ToList();
        }

        public void DeleteHistory(Guid reportId)
        {
            _connectionFactory().Execute(@"
                    DELETE [dbo].[AuditHistory]
                     WHERE ReportId = @ReportId",
                    new {reportId}, _transactionFactory());
        }
    }
}