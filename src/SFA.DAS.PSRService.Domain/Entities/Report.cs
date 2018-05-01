using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.PSRService.Domain.Entities
{
    public class Report
    {
        public Guid Id { get; set; }
        public string OrganisationName { get; set; }
        public string EmployerId { get; set; }
        public IEnumerable<Section> Sections { get; set; }
        public bool Submitted { get; set; }
        public string ReportingPeriod { get; set; }
        public Submitted SubmittedDetails { get; set; }
        public ReportingPercentages ReportingPercentages {get; set; }


        public Section GetQuestionSection(string sectionId)
        {
            var sectionsList = GetSections();
            return sectionsList.SingleOrDefault(w => w.Id == sectionId);
        }

        public void UpdatePercentages()
        {
            ReportingPercentages = GetPercentages();
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
            if (String.IsNullOrWhiteSpace(employeeQuestions.Questions.Single(w => w.Id == "newThisPeriod").Answer) == false)
                apprenticePeriod = decimal.Parse(apprenticeQuestions.Questions.Single(w => w.Id == "newThisPeriod").Answer);

            if (String.IsNullOrWhiteSpace(employeeQuestions.Questions.Single(w => w.Id == "atEnd").Answer) == false)
                employmentEnd = decimal.Parse(employeeQuestions.Questions.Single(w => w.Id == "atEnd").Answer);
            if (String.IsNullOrWhiteSpace(employeeQuestions.Questions.Single(w => w.Id == "atEnd").Answer) == false)
                apprenticeEnd = decimal.Parse(apprenticeQuestions.Questions.Single(w => w.Id == "atEnd").Answer);

            if (String.IsNullOrWhiteSpace(employeeQuestions.Questions.Single(w => w.Id == "atStart").Answer) == false)
                employmentStart = decimal.Parse(employeeQuestions.Questions.Single(w => w.Id == "atStart").Answer);


            if (apprenticePeriod != 0 & employmentPeriod != 0)
                percentages.EmploymentStarts = (apprenticePeriod / employmentPeriod) * 100;

            if (apprenticeEnd != 0 & employmentEnd != 0)
                percentages.TotalHeadCount = (apprenticeEnd / employmentEnd) * 100;

            if (apprenticePeriod != 0 & employmentStart != 0)
                percentages.NewThisPeriod = (apprenticePeriod / employmentStart) * 100;


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

        private IEnumerable<Section> GetSections(Section section)
        {
            List<Section> sectionList = new List<Section> {section};

            if (section.SubSections != null)
            {
                foreach (var reportSection in section.SubSections)
                {

                    sectionList.AddRange(GetSections(reportSection));
                }
            }


            return sectionList;
        }

    }
}
