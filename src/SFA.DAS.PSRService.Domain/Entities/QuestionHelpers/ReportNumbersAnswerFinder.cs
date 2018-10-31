using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.PSRService.Domain.Entities.QuestionHelpers
{
    public abstract class ReportNumbersAnswerFinder
    {
        private readonly IEnumerable<Question> _questions;

        public ReportNumbersAnswerFinder(IEnumerable<Question> questions)
        {
            _questions = questions != null ? questions:new List<Question>(0);
        }

        public string AtStart => _questions.Single(q => q.Id.Equals("atStart")).Answer;
        public string AtEnd => _questions.Single(q => q.Id.Equals("atEnd")).Answer;
        public string NewThisPeriod => _questions.Single(q => q.Id.Equals("newThisPeriod")).Answer;

    }
}