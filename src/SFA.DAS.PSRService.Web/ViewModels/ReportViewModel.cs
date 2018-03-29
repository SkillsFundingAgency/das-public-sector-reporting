using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.ViewModels
{
    public class ReportViewModel
    {
        public Report Report { get; set; }

        public string CurrentPeriod { get; set; }
        public bool SubmitValid { get; set; }
        public ReportingPercentages Percentages { get; set; }
        public CurrentPeriod Period { get; set; }
    }
}
