namespace SFA.DAS.PSRService.Api.Types
{
    public class PsrsStatisticsUpdatedMessage
    {
        public long Id { get; set; }
        public int SubmittedTotals { get; set; }
        public int InProcessTotals { get; set; }
        public int ViewedTotals { get; set; }
        public int ReportingPeriod { get; set; }
    }
}
