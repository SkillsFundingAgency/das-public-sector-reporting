using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.Attributes;

namespace SFA.DAS.PSRService.Web.ViewModels
{
    public class QuestionViewModel
    {
        public string Id { get; set; }

        [CustomAnswerValidation]
        public string Answer { get; set; }
        public bool Optional { get; set; }
        public QuestionType Type { get; set; }

        //public ModelStateDictionary ModelStateErrors { get; set; }
    }

    
}
