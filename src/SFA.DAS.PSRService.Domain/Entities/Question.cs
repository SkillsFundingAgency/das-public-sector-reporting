using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SFA.DAS.PSRService.Domain.Entities
{
    using System;
    using Enums;

    public class Question
    {
       public string Id { get; set; }
       public string Answer { get; set; }
       public bool Optional { get; set; }
       public  QuestionType Type { get; set; }
    }
}
