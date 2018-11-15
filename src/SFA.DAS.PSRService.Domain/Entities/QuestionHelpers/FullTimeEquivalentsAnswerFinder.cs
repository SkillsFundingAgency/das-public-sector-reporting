using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.PSRService.Domain.Entities.QuestionHelpers
{
    public static class FullTimeEquivalentsAnswerFinder
    {
        public static IReportNumbersAnswerFinder BuildFullTimeEquivalentsAnswerFinderForSections(
            IEnumerable<Section> sections)
        {
            var fulltimeEquivalentsQuestions =
                extractFulltimeEquivalentsQuestions(sections);

            if (fulltimeEquivalentsQuestions.Any())
                return new ReportNumbersAnswerFinder(fulltimeEquivalentsQuestions);

            return new AlwaysZeroReportNumbersAnswerFinder();
        }

        private static IEnumerable<Question> extractFulltimeEquivalentsQuestions(IEnumerable<Section> sections)
        {
            var reportNumbers = sections
                .SingleOrDefault(s => s.Id.Equals("ReportNumbers"));

            if (reportNumbers == null) return Enumerable.Empty<Question>();

            var fulltimeEquivalents =
                reportNumbers
                    .SubSections
                    .SingleOrDefault(ss => ss.Id.Equals("FullTimeEquivalent"));

            if (fulltimeEquivalents == null) return Enumerable.Empty<Question>();

            return
                fulltimeEquivalents
                    .Questions;
        }
    }
}