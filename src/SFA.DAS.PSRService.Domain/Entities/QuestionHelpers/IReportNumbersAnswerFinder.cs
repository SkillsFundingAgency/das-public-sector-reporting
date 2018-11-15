namespace SFA.DAS.PSRService.Domain.Entities.QuestionHelpers
{
    public interface IReportNumbersAnswerFinder
    {
        string AtStart { get; }
        string AtEnd { get; }
        string NewThisPeriod { get; }
    }
}