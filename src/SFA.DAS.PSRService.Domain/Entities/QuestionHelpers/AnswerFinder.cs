using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.PSRService.Domain.Entities.QuestionHelpers
{
    public class AnswerFinder
    {
        private readonly IEnumerable<Section> _sections;
        private Lazy<string> _fullTimeEquivalents;
        private Lazy<ReportNumbersAnswerFinder> yourEmployeesAnswers;
        private Lazy<ReportNumbersAnswerFinder> yourApprenticesAnswers;
        private FactorsAnswerFinder _factorsAnswerFinder;

        public AnswerFinder(IEnumerable<Section> sections)
        {
            _sections = sections != null? sections : new List<Section>(0);

            yourEmployeesAnswers = new Lazy<ReportNumbersAnswerFinder>( () => new YourEmployeesAnswerFinder(_sections));
            yourApprenticesAnswers = new Lazy<ReportNumbersAnswerFinder>( () => new YourApprenticesAnswerFinder(_sections));

            _fullTimeEquivalents = new Lazy<string>(findFullTimeEquivalentsAnswer);

            _factorsAnswerFinder = new FactorsAnswerFinder(_sections.Single( s => s.Id.Equals("Factors")));
        }

        private string findFullTimeEquivalentsAnswer()
        {
            return
                new FullTimeEquivalentsAnswerFinder(_sections).AtStart;
        }

        public ReportNumbersAnswerFinder YourEmployees => yourEmployeesAnswers.Value;
        public ReportNumbersAnswerFinder YourApprentices => yourApprenticesAnswers.Value;
        public string FullTimeEquivalents => _fullTimeEquivalents.Value;
        public string OutlineActions => _factorsAnswerFinder.OutlineActions.Answer;
        public string Challenges => _factorsAnswerFinder.Challenges.Answer;
        public string TargetPlans => _factorsAnswerFinder.TargetPlans.Answer;
        public string AnythingElse => _factorsAnswerFinder.AnythingElse.Answer;
    }
}