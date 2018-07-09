using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.ViewModels
{
    public class ReportViewModel : IValidatableObject
    {
        public Report Report { get; set; }

        public bool IsValidForSubmission { get; set; }

        public PercentagesViewModel Percentages { get; set; }

        public Period Period { get; set; }

        public bool CanBeEdited { get; set; }
        public bool UserCanSubmitReports { get; set; }
        public bool UserCanEditReport { get; set; }
        public string Subtitle { get; set; }
        public bool IsReadOnly { get; set; }

        public ReportViewModel()
        {
            Subtitle = String.Empty;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (Report.IsValidForSubmission() || Report.Sections == null)
                return validationResults;

            foreach (var summaryText in Report.GetNamesOfIncompleteMandatoryQuestionSections())
            {
                validationResults.Add(new ValidationResult($"{summaryText} questions are mandatory"));
            }

            return validationResults;
        }
    }
}
