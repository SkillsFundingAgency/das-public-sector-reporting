using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.ViewModels
{
    public class QuestionViewModel
    {
        public Report Report { get; set; }

        public Section CurrentSection { get; set; }

        //public ModelStateDictionary ModelStateErrors { get; set; }
    }

    
}
