namespace SFA.DAS.PSRService.Domain.Entities.QuestionHelpers
{
    public class AnythingElseAnswerFinder
        :FactorAnswerFinder
    {
        public AnythingElseAnswerFinder(Section factorSection)
            : base(factorSection, "AnythingElse")
        {
        }
    }
}