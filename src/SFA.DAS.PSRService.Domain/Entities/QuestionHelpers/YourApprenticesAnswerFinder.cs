using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.PSRService.Domain.Entities.QuestionHelpers
{
    public static class YourApprenticesAnswerFinder
    {
        public static IReportNumbersAnswerFinder BuildYourApprenticesAnswerForSections(IEnumerable<Section> sections)
        {
            var yourApprenticesQuestions =
                extractYourApprenticesQuestions(sections);

            if (yourApprenticesQuestions.Any())
                return new ReportNumbersAnswerFinder(yourApprenticesQuestions);

            return new AlwaysZeroReportNumbersAnswerFinder();
        }

        private static IEnumerable<Question> extractYourApprenticesQuestions(IEnumerable<Section> sections)
        {
            var reportNumbers = sections
                .SingleOrDefault(s => s.Id.Equals("ReportNumbers"));

            if (reportNumbers == null) return Enumerable.Empty<Question>();

            var yourApprentices =
                reportNumbers
                    .SubSections
                    .SingleOrDefault(ss => ss.Id.Equals("YourApprentices"));

            if (yourApprentices == null) return Enumerable.Empty<Question>();

            return
                yourApprentices
                    .Questions;
        }
    }
}