using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.Services
{
    public interface IReportService
    {
        Report CreateReport(long employerId);
        Report GetReport(Guid reportId);
        void SubmitReport(int reportId);
        IList<Report> GetReports(long employerId);
    }
}