using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;

namespace SFA.DAS.PSRService.Web.Services
{
    public interface IReportService
    {
        Report CreateReport(long employerId);
        Report GetReport(string period,long employerId);
        SubmittedStatus SubmitReport(string period, long employerId, Submitted submittedDetails);
        IEnumerable<Report> GetSubmittedReports(long employerId);
        bool IsSubmitValid(Report report);
        Section GetQuestionSection(string SectionId, Report report);
        string GetCurrentReportPeriod();
        string GetCurrentReportPeriodName(string period);
        bool IsSubmissionsOpen();
    }
}