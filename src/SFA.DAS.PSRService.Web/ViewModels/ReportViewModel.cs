using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.ViewModels
{
    public class ReportViewModel
    {
        public Report Report { get; set; }

        public string CurrentPeriod { get; set; }
        public bool SubmitValid { get; set; }
        public ReportingPercentages Percentages { get; set; }
    }
}
