﻿using System;
using System.Collections.Generic;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.Services
{
    public interface IReportService
    {
        Report CreateReport(string employerId);
        Report GetReport(string period,string employerId);
        SubmittedStatus SubmitReport(string period, string employerId, Submitted submittedDetails);
        IEnumerable<Report> GetSubmittedReports(string employerId);
        void SaveReport(Report report);

    }
}