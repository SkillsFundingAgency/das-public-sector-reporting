using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;

namespace SFA.DAS.PSRService.Data;

public class SqlReportRepository(IDbConnection connection) : IReportRepository
{
    public async Task<ReportDto> Get(string period, string employerId)
    {
        return await connection.QuerySingleOrDefaultAsync<ReportDto>(
            "select * from Report where EmployerID = @employerId and ReportingPeriod = @period",
            new { employerId, period });
    }

    public async Task<ReportDto> Get(Guid id)
    {
        return await connection.QuerySingleOrDefaultAsync<ReportDto>("select * from Report where Id = @id", new { id });
    }

    public async Task<List<ReportDto>> GetSubmitted(string employerId)
    {
        var reportData = await connection.QueryAsync<ReportDto>(
            "select * from dbo.Report where EmployerID = @employerId and Submitted = 1",
            new { employerId });

        return reportData.ToList();
    }

    public async Task Create(ReportDto reportDto)
    {
        await connection.ExecuteAsync(@"
                    INSERT INTO [dbo].[Report] ([Id],[EmployerId],[ReportingPeriod],[ReportingData],[Submitted],[AuditWindowStartUtc],[UpdatedUtc],[UpdatedBy])
                                        VALUES (@Id, @EmployerId, @ReportingPeriod, @ReportingData, @Submitted, @AuditWindowStartUtc, @UpdatedUtc, @UpdatedBy)",
            new
            {
                reportDto.Id,
                reportDto.EmployerId,
                reportDto.ReportingData,
                reportDto.ReportingPeriod,
                reportDto.Submitted,
                reportDto.AuditWindowStartUtc,
                reportDto.UpdatedUtc,
                reportDto.UpdatedBy
            });
    }

    public async Task Update(ReportDto reportDto)
    {
        await connection.ExecuteAsync(@"
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
            });
    }

    public async Task SaveAuditRecord(AuditRecordDto auditRecordDto)
    {
        await connection.ExecuteAsync(@"
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
            new
            {
                auditRecordDto.UpdatedUtc,
                auditRecordDto.ReportingData,
                auditRecordDto.UpdatedBy,
                auditRecordDto.ReportId
            });
    }

    public async Task<IReadOnlyList<AuditRecordDto>> GetAuditRecordsMostRecentFirst(Guid reportId)
    {
        var records = await connection
            .QueryAsync<AuditRecordDto>(
                "select * from dbo.AuditHistory where ReportId = @reportId order by UpdatedUtc desc",
                new { reportId });

        return records.ToList();
    }

    public async Task DeleteHistory(Guid reportId)
    {
        await connection.ExecuteAsync(@"
                    DELETE [dbo].[AuditHistory]
                     WHERE ReportId = @ReportId",
            new { reportId });
    }
}