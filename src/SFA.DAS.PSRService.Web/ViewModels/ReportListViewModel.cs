using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.ViewModels
{
    public class ReportListViewModel
    {
        public IEnumerable<Report>SubmittedReports { get; set; }
        public Dictionary<string,CurrentPeriod> Periods { get; set; }
    }
}
