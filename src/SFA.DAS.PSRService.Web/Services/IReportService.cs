using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Services;

public interface IReportService
{
    Task CreateReport(string employerId, UserModel user, bool? isLocalAuthority);
    Task<Report> GetReport(string period,string employerId);
    Task SubmitReport(Report report);
    Task<IEnumerable<Report>> GetSubmittedReports(string employerId);
    Task SaveReport(Report report, UserModel userModel,bool? isLocalAuthority);
    bool CanBeEdited(Report report);
    Task<IEnumerable<AuditRecord>> GetReportEditHistoryMostRecentFirst(Period period, string employerId);
}