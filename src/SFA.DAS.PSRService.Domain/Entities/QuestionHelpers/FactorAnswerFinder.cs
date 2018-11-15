using System;
using System.Linq;

namespace SFA.DAS.PSRService.Domain.Entities.QuestionHelpers
{
    public abstract class FactorAnswerFinder : IFactorAnswerFinder
    {
        private readonly Section _factorSection;
        private readonly string _subsectionAndQuestionId;
        private Lazy<string> _answer;

        public FactorAnswerFinder(
            Section factorSection,
            string subsectionAndQuestionId)
        {
            if (string.IsNullOrWhiteSpace(subsectionAndQuestionId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(subsectionAndQuestionId));

            _factorSection = factorSection ?? throw new ArgumentNullException(nameof(factorSection));
            _subsectionAndQuestionId = subsectionAndQuestionId;

            _answer = new Lazy<string>(findAnswer);
        }

        private string findAnswer()
        {
            return 

            _factorSection
                .SubSections
                .Single(ss => ss.Id.Equals(_subsectionAndQuestionId))
                .Questions
                .Single(q => q.Id.Equals(_subsectionAndQuestionId))
                .Answer;
        }

        public string Answer => _answer.Value;
    }
}