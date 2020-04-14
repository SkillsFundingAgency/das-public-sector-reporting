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
        public string ZenDeskLabel => GetZendeskLabel_ByQuestionId(Id);

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

        private string GetZendeskLabel_ByQuestionId(string id)
        {
            switch (id)
            {
                case "OutlineActions":
                    return "reporting-what-actions-have you-taken";
                case "Challenges":
                    return "reporting-what-challenges-have-you-faced";
                case "TargetPlans":
                    return "reporting-planning-to-meet-the-target";
                case "AnythingElse":
                    return "reporting-anything-else-you-want-to-tell";
                case "YourApprentices":
                    return "reporting-your-apprentices";
                case "YourEmployees":
                    return "reporting-your-employees";
                case "FullTimeEquivalent":
                    return "reporting-full-time-equivalents";
            }
            return string.Empty;
        }
    }
}
