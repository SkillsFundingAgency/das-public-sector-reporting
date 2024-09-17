namespace SFA.DAS.PSRService.Messages.Events;

public class ReportingPercentages 
{
    public ReportingPercentages()
    {
        EmploymentStarts = "0.00";
        TotalHeadCount = "0.00";
        NewThisPeriod = "0.00";
    }
    public string EmploymentStarts { get; set; }
    public string TotalHeadCount { get; set; }
    public string NewThisPeriod { get; set; }
}