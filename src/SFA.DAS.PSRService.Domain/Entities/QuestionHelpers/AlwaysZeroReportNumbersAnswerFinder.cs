namespace SFA.DAS.PSRService.Domain.Entities.QuestionHelpers
{
    public sealed class AlwaysZeroReportNumbersAnswerFinder 
        : IReportNumbersAnswerFinder
    {
        public string AtStart => "0.00";
        public string AtEnd => "0.00";
        public string NewThisPeriod => "o.00";
    }
}