using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.PSRService.Domain.Entities
{
    public class Report
    {
        public Guid Id { get; set; }
        public string OrganisationName { get; set; }
        public bool? HasTotalEmployeesMeetMinimum { get; set; }
        public string TotalEmployees { get; set; }
        public bool? IsLocalAuthority { get; set; }
        public string EmployerId { get; set; }
        public IEnumerable<Section> Sections { get; set; }
        public bool Submitted { get; set; }
        public string SerialNo { get; set; }
        public string ReportingPeriod { get; set; }
        public Submitted SubmittedDetails { get; set; }
        public ReportingPercentages ReportingPercentages { get; set; }
        public ReportingPercentages ReportingPercentagesSchools { get; set; }
        public Period Period { get; set; }
        public DateTime? AuditWindowStartUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public User UpdatedBy { get; set; }

        public bool IsValidForSubmission()
        {
            return
                ReportIsNotYetSubmitted()
                && AllSectionsAreValid()
            && OrganisationNameIsValid()
            && HasTotalEmployeesMeetMinimumIsValid()
            && HasMinimumTotalEmployees();
        }

        private bool OrganisationNameIsValid()
        {
            return string.IsNullOrWhiteSpace(OrganisationName) == false;
        }

        private bool HasMinimumTotalEmployees()
        {
            if (!IsLocalAuthority.HasValue)
                return true;

            return string.IsNullOrEmpty(TotalEmployees) ? false : int.Parse(TotalEmployees) < 250 == false;
        }

        private bool HasTotalEmployeesMeetMinimumIsValid()
        {
            if (!IsLocalAuthority.HasValue)
                return true;

            return HasTotalEmployeesMeetMinimum.HasValue;
        }

        private bool ReportIsNotYetSubmitted()
        {
            return Submitted == false;
        }

        private bool AllSectionsAreValid()
        {
            return Sections == null || Sections.All(s => s.IsValidForSubmission());
        }

        public Section GetQuestionSection(string sectionId)
        {
            var sectionsList = GetSections();

            return sectionsList.SingleOrDefault(w => w.Id == sectionId);
        }

        public void UpdatePercentages()
        {
            ReportingPercentages = GetPercentages();
            ReportingPercentagesSchools = GetPercentagesSchools();
        }

        private ReportingPercentages GetPercentages()
        {
            var percentages = new ReportingPercentages();

            if (Sections == null)
                return null;

            var employeeQuestions = GetQuestionSection("YourEmployees");

            if (employeeQuestions == null)
                return null;

            var apprenticeQuestions = GetQuestionSection("YourApprentices");

            if (apprenticeQuestions == null)
                return null;


            decimal employmentPeriod = 0, apprenticePeriod = 0, employmentEnd = 0, apprenticeEnd = 0, employmentStart = 0;

            if (String.IsNullOrWhiteSpace(employeeQuestions.Questions.Single(w => w.Id == "newThisPeriod").Answer) == false)
                employmentPeriod = decimal.Parse(employeeQuestions.Questions.Single(w => w.Id == "newThisPeriod").Answer);
            if (String.IsNullOrWhiteSpace(apprenticeQuestions.Questions.Single(w => w.Id == "newThisPeriod").Answer) == false)
                apprenticePeriod = decimal.Parse(apprenticeQuestions.Questions.Single(w => w.Id == "newThisPeriod").Answer);

            if (String.IsNullOrWhiteSpace(employeeQuestions.Questions.Single(w => w.Id == "atEnd").Answer) == false)
                employmentEnd = decimal.Parse(employeeQuestions.Questions.Single(w => w.Id == "atEnd").Answer);
            if (String.IsNullOrWhiteSpace(apprenticeQuestions.Questions.Single(w => w.Id == "atEnd").Answer) == false)
                apprenticeEnd = decimal.Parse(apprenticeQuestions.Questions.Single(w => w.Id == "atEnd").Answer);

            if (String.IsNullOrWhiteSpace(employeeQuestions.Questions.Single(w => w.Id == "atStart").Answer) == false)
                employmentStart = decimal.Parse(employeeQuestions.Questions.Single(w => w.Id == "atStart").Answer);


            if (apprenticePeriod != 0 & employmentPeriod != 0)
                percentages.EmploymentStarts = ((apprenticePeriod / employmentPeriod) * 100).ToString("F2");

            if (apprenticeEnd != 0 & employmentEnd != 0)
                percentages.TotalHeadCount = ((apprenticeEnd / employmentEnd) * 100).ToString("F2");

            if (apprenticePeriod != 0 & employmentStart != 0)
                percentages.NewThisPeriod = ((apprenticePeriod / employmentStart) * 100).ToString("F2");

            if (IsLocalAuthority.HasValue)
                TotalEmployees = (employmentPeriod + employmentEnd).ToString("F0");

            if (ReportingPercentages != null)
                percentages.Title = ReportingPercentages.Title;

            return percentages;

        }
        private ReportingPercentages GetPercentagesSchools()
        {
            var percentages = new ReportingPercentages();

            if (Sections == null)
                return null;

            var employeeQuestions = GetQuestionSection("SchoolsEmployees");

            if (employeeQuestions == null)
                return null;

            var apprenticeQuestions = GetQuestionSection("SchoolsApprentices");

            if (apprenticeQuestions == null)
                return null;


            decimal employmentPeriod = 0, apprenticePeriod = 0, employmentEnd = 0, apprenticeEnd = 0, employmentStart = 0;

            if (String.IsNullOrWhiteSpace(employeeQuestions.Questions.Single(w => w.Id == "newThisPeriod").Answer) == false)
                employmentPeriod = decimal.Parse(employeeQuestions.Questions.Single(w => w.Id == "newThisPeriod").Answer);
            if (String.IsNullOrWhiteSpace(apprenticeQuestions.Questions.Single(w => w.Id == "newThisPeriod").Answer) == false)
                apprenticePeriod = decimal.Parse(apprenticeQuestions.Questions.Single(w => w.Id == "newThisPeriod").Answer);

            if (String.IsNullOrWhiteSpace(employeeQuestions.Questions.Single(w => w.Id == "atEnd").Answer) == false)
                employmentEnd = decimal.Parse(employeeQuestions.Questions.Single(w => w.Id == "atEnd").Answer);
            if (String.IsNullOrWhiteSpace(apprenticeQuestions.Questions.Single(w => w.Id == "atEnd").Answer) == false)
                apprenticeEnd = decimal.Parse(apprenticeQuestions.Questions.Single(w => w.Id == "atEnd").Answer);

            if (String.IsNullOrWhiteSpace(employeeQuestions.Questions.Single(w => w.Id == "atStart").Answer) == false)
                employmentStart = decimal.Parse(employeeQuestions.Questions.Single(w => w.Id == "atStart").Answer);


            if (apprenticePeriod != 0 & employmentPeriod != 0)
                percentages.EmploymentStarts = ((apprenticePeriod / employmentPeriod) * 100).ToString("F2");

            if (apprenticeEnd != 0 & employmentEnd != 0)
                percentages.TotalHeadCount = ((apprenticeEnd / employmentEnd) * 100).ToString("F2");

            if (apprenticePeriod != 0 & employmentStart != 0)
                percentages.NewThisPeriod = ((apprenticePeriod / employmentStart) * 100).ToString("F2");

            percentages.Title = ReportingPercentagesSchools.Title;

            return percentages;

        }

        private IEnumerable<Section> GetSections()
        {
            var sectionList = new List<Section>();

            if (Sections == null)
                return sectionList;

            foreach (var reportSection in Sections)
            {
                sectionList.AddRange(GetSections(reportSection));
            }

            return sectionList;
        }

        private static IEnumerable<Section> GetSections(Section section)
        {
            List<Section> sectionList = new List<Section> { section };

            if (section.SubSections != null)
            {
                foreach (var reportSection in section.SubSections)
                {
                    sectionList.AddRange(GetSections(reportSection));
                }
            }


            return sectionList;
        }

        public IEnumerable<string> GetNamesOfIncompleteMandatoryQuestionSections()
        {
            if (OrganisationNameIsValid() == false)
                yield return @"Organisation";

            if (IsLocalAuthority.HasValue)
            {
                if (HasTotalEmployeesMeetMinimumIsValid() == false)
                    yield return @"TotalEmployees";
            }

            foreach (var text in GetSummaryTextFromFirstLevelSubSectionsNotValidForSubmission())
            {
                yield return text;
            }
        }

        public IEnumerable<string> GetNamesOfFailedValidations()
        {
            if (HasMinimumTotalEmployees() == false)
                yield return @"You do not have to submit a report because your organisation has less than 250 employees";
        }

        private IEnumerable<string> GetSummaryTextFromFirstLevelSubSectionsNotValidForSubmission()
        {
            return
                Sections
                    .SelectMany(s => s.SubSections)
                    .Where(w => !w.IsValidForSubmission())
                    .Select(subSection => subSection.SummaryText);
        }
    }
}
