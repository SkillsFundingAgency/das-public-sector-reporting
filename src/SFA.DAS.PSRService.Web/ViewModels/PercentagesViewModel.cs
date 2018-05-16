using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.ViewModels
{
    public class PercentagesViewModel
    {
        public PercentagesViewModel(ReportingPercentages percentages)
        {
            if (percentages == null)
                return;

            EmploymentStarts = percentages.EmploymentStarts.ToString("0.00") + "%";
            TotalHeadCount = percentages.TotalHeadCount.ToString("0.00") + "%";
            NewThisPeriod = percentages.NewThisPeriod.ToString("0.00") + "%";
        }

        public string EmploymentStarts { get; }
        public string TotalHeadCount { get; }
        public string NewThisPeriod { get; }
       
    }
}
