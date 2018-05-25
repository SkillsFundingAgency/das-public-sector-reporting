using MediatR;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class GetAuditHistoryRequest : IRequest<AuditRecord>
    {
    }
}