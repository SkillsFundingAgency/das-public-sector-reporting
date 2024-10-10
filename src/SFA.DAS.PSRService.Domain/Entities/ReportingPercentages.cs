namespace SFA.DAS.PSRService.Domain.Entities;

public class ReportingPercentages 
{
    public ReportingPercentages()
    {
        EmploymentStarts = "0.00";
        TotalHeadCount = "0.00";
        NewThisPeriod = "0.00";
        Title = "ReportingPercentages";
    }
    public string EmploymentStarts { get; set; }
    public string TotalHeadCount { get; set; }
    public string NewThisPeriod { get; set; }
    public string Title { get; set; }
}