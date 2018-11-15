using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.PSRService.Domain.Entities.QuestionHelpers
{
    public static class YourEmployeesAnswerFinder
    {
        public static IReportNumbersAnswerFinder BuildYourEmployeesAnswerFinderForSections(
            IEnumerable<Section> sections)
        {
            var yourEmployeesQuestions =
                extractYourEmployeesQuestions(sections);

            if (yourEmployeesQuestions.Any())
                return new ReportNumbersAnswerFinder(yourEmployeesQuestions);

            return new AlwaysZeroReportNumbersAnswerFinder();
        }

        private static IEnumerable<Question> extractYourEmployeesQuestions(IEnumerable<Section> sections)
        {
            var reportNumbers = sections
                .SingleOrDefault(s => s.Id.Equals("ReportNumbers"));

            if(reportNumbers == null) return Enumerable.Empty<Question>();

            var yourEmployees =
                reportNumbers
                .SubSections
                .SingleOrDefault(ss => ss.Id.Equals("YourEmployees"));

            if (yourEmployees == null) return Enumerable.Empty<Question>();

            return
                yourEmployees
                    .Questions;
        }
    }
}