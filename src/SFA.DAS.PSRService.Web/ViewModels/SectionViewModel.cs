using System.Collections.Generic;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.ViewModels
{
    public class SectionViewModel
    {
        public Report Report { get; set; }

        public Section CurrentSection { get; set; }

        public IList<QuestionViewModel> Questions { get; set; }

        public Period CurrentPeriod { get; set; }
        
    }
}
