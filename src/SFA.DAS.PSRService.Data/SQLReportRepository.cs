using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Dapper;
using SFA.DAS.NServiceBus;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;

namespace SFA.DAS.PSRService.Data
{
    public class SQLReportRepository : IReportRepository
    {
        private readonly IUnitOfWorkContext _unitOfWork;

        public SQLReportRepository(IUnitOfWorkContext unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ReportDto Get(string period, string employerId)
        {
            var connection = _unitOfWork.Get<DbConnection>();
            var report = connection.QuerySingleOrDefault<ReportDto>("select * from Report where EmployerID = @employerId and ReportingPeriod = @period",
                         new {employerId, period});

            return report;
            
        }

        public ReportDto Get(Guid id)
        {
            var connection = _unitOfWork.Get<DbConnection>();
            return connection.QuerySingleOrDefault<ReportDto>("select * from Report where Id = @id", new {id});
        }

        public IList<ReportDto> GetSubmitted(string employerId)
        {
        var connection = _unitOfWork.Get<DbConnection>();
        var reportData = connection.Query<ReportDto>("select * from dbo.Report where EmployerID = @employerId and Submitted = 1",
                         new {employerId});
        return reportData.ToList();
        }

        public void Create(ReportDto report)
        {
            var connection = _unitOfWork.Get<DbConnection>();
            var transaction = _unitOfWork.Get<DbTransaction>();
            connection.Execute(@"
                    INSERT INTO [dbo].[Report] ([Id],[EmployerId],[ReportingPeriod],[ReportingData],[Submitted],[AuditWindowStartUtc],[UpdatedUtc],[UpdatedBy])
                                        VALUES (@Id, @EmployerId, @ReportingPeriod, @ReportingData, @Submitted, @AuditWindowStartUtc, @UpdatedUtc, @UpdatedBy)",
                    new {report.Id, report.EmployerId, report.ReportingData, report.ReportingPeriod, report.Submitted, report.AuditWindowStartUtc, report.UpdatedUtc, report.UpdatedBy}, transaction);
        }

        public void Update(ReportDto reportDto)
        {
        var connection = _unitOfWork.Get<DbConnection>();
        var transaction = _unitOfWork.Get<DbTransaction>();
            connection.Execute(@"
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
                }, transaction);
        }

        public void SaveAuditRecord(AuditRecordDto auditRecordDto)
        {
            var connection = _unitOfWork.Get<DbConnection>();
            var transaction = _unitOfWork.Get<DbTransaction>();
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
                    new {auditRecordDto.UpdatedUtc, auditRecordDto.ReportingData, auditRecordDto.UpdatedBy, auditRecordDto.ReportId}, transaction);
        }

        public IReadOnlyList<AuditRecordDto> GetAuditRecordsMostRecentFirst(Guid reportId)
        {
            var connection = _unitOfWork.Get<DbConnection>();
            return connection.Query<AuditRecordDto>("select * from dbo.AuditHistory where ReportId = @reportId order by UpdatedUtc desc", new {reportId}).ToList();
                  
        }

        public void DeleteHistory(Guid reportId)
        {
            var connection = _unitOfWork.Get<DbConnection>();
            var transaction = _unitOfWork.Get<DbTransaction>();
            connection.Execute(@"
                    DELETE [dbo].[AuditHistory]
                     WHERE ReportId = @ReportId",
                    new {reportId}, transaction);
        }
    }
}