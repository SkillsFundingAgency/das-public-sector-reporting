namespace SFA.DAS.PSRService.Domain.Entities.QuestionHelpers
{
    public class ChallengesAnswerFinder
        :FactorAnswerFinder
    {
        public ChallengesAnswerFinder(Section factorSection)
            : base(factorSection, "Challenges")
        {
        }
    }
}