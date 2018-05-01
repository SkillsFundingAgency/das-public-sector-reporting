using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.ViewModels
{
    public class ReportViewModel : IValidatableObject
    {
        public Report Report { get; set; }

        public string CurrentPeriod { get; set; }
        public bool SubmitValid { get; set; }
        public PercentagesViewModel Percentages { get; set; }
        public CurrentPeriod Period { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            if (Report.Sections.Where(w => w.CompletionStatus != CompletionStatus.Optional)
                .All(s => s.CompletionStatus == CompletionStatus.Completed))
            {
                return validationResults;
            }
            else
            {
                if (Report?.Sections != null)
                {
                    foreach (var section in Report.Sections.SelectMany(s => s.SubSections).Where(w =>
                        w.CompletionStatus != CompletionStatus.Optional &&
                        w.CompletionStatus != CompletionStatus.Completed))
                    {
                        validationResults.Add(new ValidationResult($"{section.SummaryText} questions are mandatory"));
                    }
                }

                return validationResults;
            }
                    
        }
    }
}
