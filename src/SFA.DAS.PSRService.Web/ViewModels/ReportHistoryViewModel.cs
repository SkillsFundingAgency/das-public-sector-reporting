using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.ViewModels;

public class ReportHistoryViewModel
{
    public Period Period { get; set; }
    public IEnumerable<AuditRecord> EditHistoryMostRecentFirst { get; set; }
}