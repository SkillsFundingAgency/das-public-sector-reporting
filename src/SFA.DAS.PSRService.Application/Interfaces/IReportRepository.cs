using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.PSRService.Application.Domain;

namespace SFA.DAS.PSRService.Application.Interfaces;

public interface IReportRepository
{
    Task<ReportDto> Get(string period, string employerId);
    Task<ReportDto> Get(Guid id);
    Task<IList<ReportDto>> GetSubmitted(string employerId);
    Task Create(ReportDto reportDto);
    Task Update(ReportDto reportDto);
    Task SaveAuditRecord(AuditRecordDto auditRecordDto);
    Task<IReadOnlyList<AuditRecordDto>> GetAuditRecordsMostRecentFirst(Guid reportId);
    Task DeleteHistory(Guid reportId);
}