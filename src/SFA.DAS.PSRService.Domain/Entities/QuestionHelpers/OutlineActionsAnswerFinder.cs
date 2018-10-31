namespace SFA.DAS.PSRService.Domain.Entities.QuestionHelpers
{
    public class OutlineActionsAnswerFinder
    :FactorAnswerFinder
    {
        public OutlineActionsAnswerFinder(Section factorSection)
            : base(factorSection, "OutlineActions")
        {
        }
    }
}