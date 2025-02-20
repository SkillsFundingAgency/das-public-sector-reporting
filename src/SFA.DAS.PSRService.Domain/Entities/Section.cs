using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.PSRService.Domain.Entities;

public class Section
{
    public IEnumerable<Section> SubSections { get; set; }
    public IEnumerable<Question> Questions { get; set; }
    public string Id { get; set; }
    public string Title { get; set; }
    public string SummaryText { get; set; }
    public string SerialNo { get; set; }
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

    private static string GetZendeskLabel_ByQuestionId(string questionId)
    {
        return questionId switch
        {
            "OutlineActions" => "reporting-what-actions-have you-taken",
            "Challenges" => "reporting-what-challenges-have-you-faced",
            "TargetPlans" => "reporting-planning-to-meet-the-target",
            "AnythingElse" => "reporting-anything-else-you-want-to-tell",
            "YourApprentices" => "reporting-your-apprentices",
            "YourEmployees" => "reporting-your-employees",
            "FullTimeEquivalent" => "reporting-full-time-equivalents",
            "SchoolsApprentices" => "reporting-Schools-apprentices",
            "SchoolsEmployees" => "reporting-Schools-employees",
            _ => string.Empty
        };
    }
}