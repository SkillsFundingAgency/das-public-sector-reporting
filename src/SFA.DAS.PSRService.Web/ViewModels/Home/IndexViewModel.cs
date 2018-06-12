using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.ViewModels.Home
{
    public class IndexViewModel
    {
        public bool CurrentReportExists { get; set; }
        public bool CanCreateReport { get;set; }
        public bool CanEditReport { get; set; }
        public Period Period { get; set; }
        public bool Readonly { get; set; }
        public bool CurrentReportAlreadySubmitted { get; set; }
    }
}
