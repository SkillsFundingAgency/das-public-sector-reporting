namespace SFA.DAS.PSRService.Domain.Entities.QuestionHelpers
{
    public class TargetPlansAnswerFinder
        :FactorAnswerFinder
    {
        public TargetPlansAnswerFinder(Section factorSection)
            : base(factorSection, "TargetPlans")
        {
        }
    }
}