namespace SFA.DAS.PSRService.Domain.Entities
{
    public class ReportingPercentages 
    {
        public ReportingPercentages()
        {
            EmploymentStarts = "0";
            TotalHeadCount = "0";
            NewThisPeriod = "0";
        }
        public string EmploymentStarts { get; set; }
        public string TotalHeadCount { get; set; }
        public string NewThisPeriod { get; set; }
    }
}
