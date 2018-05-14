using System;
using System.Collections.Generic;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;

namespace SFA.DAS.PSRService.Web.Services
{
    public interface IReportService
    {
        void CreateReport(string employerId);

        [Obsolete("Use the overload that takes a Period object going forward.")]
        Report GetReport(string period, string employerId);

        Report GetReport(Period period, string employerId);
        SubmittedStatus SubmitReport(Report report);
        IEnumerable<Report> GetSubmittedReports(string employerId);
        void SaveReport(Report report);
        bool CanBeEdited(Report report);
    }
}