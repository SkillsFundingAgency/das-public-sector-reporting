using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.ViewModels
{
    public class PercentagesViewModel
    {
        public PercentagesViewModel(ReportingPercentages percentages)
        {
            if (percentages == null)
                return;

            EmploymentStarts = percentages.EmploymentStarts + "%";
            TotalHeadCount = percentages.TotalHeadCount + "%";
            NewThisPeriod = percentages.NewThisPeriod + "%";
        }

        public string EmploymentStarts { get; }
        public string TotalHeadCount { get; }
        public string NewThisPeriod { get; }
       
    }
}
