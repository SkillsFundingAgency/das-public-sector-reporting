using System;
using System.Collections.Generic;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Services
{
    public interface IReportService
    {
        void CreateReport(string employerId, UserModel user, bool? IsLocalAuthority);
        Report GetReport(string period,string employerId);
        void SubmitReport(Report report);
        IEnumerable<Report> GetSubmittedReports(string employerId);
        void SaveReport(Report report, UserModel user,bool? IsLocalAuthority);
        bool CanBeEdited(Report report);
        IEnumerable<AuditRecord> GetReportEditHistoryMostRecentFirst(Period period, string employerId);
    }
}