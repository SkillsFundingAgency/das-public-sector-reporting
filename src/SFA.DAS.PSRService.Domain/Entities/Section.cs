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

        public bool IsComplete()
        {
            return (Questions == null || Questions.All(q => !string.IsNullOrEmpty(q.Answer)))
                   && (SubSections == null || SubSections.All(s => s.IsComplete()));
        }

        public bool IsValidForSubmission()
        {
            return (Questions == null || Questions.All(q => q.Optional || !string.IsNullOrEmpty(q.Answer)))
                   && (SubSections == null || SubSections.All(s => s.IsValidForSubmission()));
        }
    }
}
