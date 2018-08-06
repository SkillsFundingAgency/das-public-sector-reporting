using SFA.DAS.PSRService.Domain.Enums;

namespace SFA.DAS.PSRService.Domain.Entities
{
    public class Question
    {
       public string Id { get; set; }
       public string Answer { get; set; }
       public bool Optional { get; set; }
       public QuestionType Type { get; set; }
    }
}
