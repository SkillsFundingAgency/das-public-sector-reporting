using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.ViewModels
{
    public class ReportingPercentages
    {
        public decimal EmploymentStarts { get; set; }

        public decimal TotalHeadCount { get; set; }
        public decimal NewThisPeriod { get; set; }
       
    }
}
