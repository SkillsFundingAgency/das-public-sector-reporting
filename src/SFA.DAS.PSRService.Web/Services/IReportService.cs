using System;
using System.Collections.Generic;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.Services
{
    public interface IReportService
    {
        void CreateReport(string employerId);
        Report GetReport(string period,string employerId);
        void SubmitReport(Report report);
        IEnumerable<Report> GetSubmittedReports(string employerId);
        void SaveReport(Report report);
        bool CanBeEdited(Report report);
    }
}