using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.PSRService.Domain.Entities.QuestionHelpers
{
    public class FullTimeEquivalentsAnswerFinder
        :ReportNumbersAnswerFinder
    {
        public FullTimeEquivalentsAnswerFinder(IEnumerable<Section> sections) :
            base(
                sections
                    .Single(s => s.Id.Equals("ReportNumbers"))
                    .SubSections
                    .Single(ss => ss.Id.Equals("FullTimeEquivalent"))
                    .Questions)
        {
        }

    }
}