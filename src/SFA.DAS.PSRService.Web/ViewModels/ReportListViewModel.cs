﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.ViewModels
{
    public class ReportListViewModel
    {
        public IEnumerable<Report>SubmittedReports { get; set; }
    }
}