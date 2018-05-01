using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.PSRService.Domain.Enums;

namespace SFA.DAS.PSRService.Domain.Entities
{
    public class Section
    {
        public IEnumerable<Section> SubSections { get; set; }
        public IEnumerable<Question> Questions { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string SummaryText { get; set; }
        public CompletionStatus CompletionStatus
        {
            get
            {
                if (SectionHasQuestions())
                {
                    return GetCompletionStatusFromQuestions();
                }
                if (SectionHasSubsections())
                {
                    return GetCompletionStatusFromSubSections();
                }
                return CompletionStatus.Completed;
            }
        }

        private CompletionStatus GetCompletionStatusFromSubSections()
        {
            if ((SubSections.Count(w => w.CompletionStatus == CompletionStatus.Completed) + SubSections.Count(x => x.CompletionStatus == CompletionStatus.Optional)) == SubSections.Count())
            {
                return CompletionStatus.Completed;
            }

            if (SubSections.Any(w => w.CompletionStatus == CompletionStatus.InProgress))
            {
                return CompletionStatus.InProgress;
            }

            return CompletionStatus.Incomplete;
        }

        private bool SectionHasSubsections()
        {
            return SubSections != null && SubSections.Any();
        }

        private CompletionStatus GetCompletionStatusFromQuestions()
        {
            if (Questions.Any(w => w.Optional && String.IsNullOrWhiteSpace(w.Answer)))
                return CompletionStatus.Optional;

            if (Questions.All(w => w.Optional == false && String.IsNullOrWhiteSpace(w.Answer) == false) || Questions.All(x => x.Optional && String.IsNullOrWhiteSpace(x.Answer) == false))
                return CompletionStatus.Completed;

            if (Questions.All(w => String.IsNullOrWhiteSpace(w.Answer)))
                return CompletionStatus.Incomplete;

            return CompletionStatus.InProgress;
        }

        private bool SectionHasQuestions()
        {
            return Questions != null && Questions.Any();
        }
    }
}
