using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.PSRService.Domain.Entities.QuestionHelpers
{
    public class AnswerFinder
    {
c        private readonly IEnumerable<Section> _sections;
        private Lazy<string> _fullTimeEquivalents;
        private Lazy<IReportNumbersAnswerFinder> yourEmployeesAnswers;
        private Lazy<IReportNumbersAnswerFinder> yourApprenticesAnswers;
        private FactorsAnswerFinder _factorsAnswerFinder;

        public AnswerFinder(IEnumerable<Section> sections)
        {
            _sections = sections ?? new List<Section>(0);

            yourEmployeesAnswers = new Lazy<IReportNumbersAnswerFinder>( () => YourEmployeesAnswerFinder.BuildYourEmployeesAnswerFinderForSections(_sections));
            yourApprenticesAnswers = new Lazy<IReportNumbersAnswerFinder>( () => YourApprenticesAnswerFinder.BuildYourApprenticesAnswerForSections(_sections));

            _fullTimeEquivalents = new Lazy<string>(findFullTimeEquivalentsAnswer);

            _factorsAnswerFinder = new FactorsAnswerFinder(_sections.SingleOrDefault( s => s.Id.Equals("Factors")));
        }

        private string findFullTimeEquivalentsAnswer()
        {
            return
                FullTimeEquivalentsAnswerFinder.BuildFullTimeEquivalentsAnswerFinderForSections(_sections).AtStart;
        }

        public IReportNumbersAnswerFinder YourEmployees => yourEmployeesAnswers.Value;
        public IReportNumbersAnswerFinder YourApprentices => yourApprenticesAnswers.Value;
        public string FullTimeEquivalents => _fullTimeEquivalents.Value;
        public string OutlineActions => _factorsAnswerFinder.OutlineActions.Answer;
        public string Challenges => _factorsAnswerFinder.Challenges.Answer;
        public string TargetPlans => _factorsAnswerFinder.TargetPlans.Answer;
        public string AnythingElse => _factorsAnswerFinder.AnythingElse.Answer;
    }
}