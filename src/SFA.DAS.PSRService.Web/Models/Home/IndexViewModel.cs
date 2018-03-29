using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.PSRService.Web.Models.Home
{
    public class IndexViewModel
    {
        public bool CanCreateReport { get;set; }
        public bool CanEditReport { get; set; }
        public string PeriodName { get; set; }
        public string DomainRootUrl { get; set; }
    }
}
