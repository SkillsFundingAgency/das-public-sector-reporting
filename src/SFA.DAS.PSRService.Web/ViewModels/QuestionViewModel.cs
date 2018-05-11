using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.Attributes;

namespace SFA.DAS.PSRService.Web.ViewModels
{
    public class QuestionViewModel
    {
        public string Id { get; set; }
        [CustomAnswerValidation("Type")]
        public string Answer { get; set; }
        public bool Optional { get; set; }
        public QuestionType Type { get; set; }
    }
}
