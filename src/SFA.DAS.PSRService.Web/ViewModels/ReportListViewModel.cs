using System.Collections.Generic;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.ViewModels
{
    public class ReportListViewModel
    {
        public IEnumerable<Report>SubmittedReports { get; set; }
        public Dictionary<string,Period> Periods { get; set; }
        public string HashedEmployerAccountId { get; set; }
    }
}
