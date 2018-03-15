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
        Report GetReport(string period,long employeeId);
        SubmittedStatus SubmitReport(string period, long employeeId, Submitted submittedDetails);
        IList<Report> GetReports(long employerId);
        bool IsSubmitValid(Report report);
        bool IsEditValid(Report report);
    }
}