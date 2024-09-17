using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.PSRService.Domain.Entities;

public class Report
{
    public Guid Id { get; set; }
    public string OrganisationName { get; set; }
    public bool? HasMinimumEmployeeHeadcount { get; set; }
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
            && HasMinimumEmployeeHeadcountIsValid();
    }

    private bool OrganisationNameIsValid()
    {
        return string.IsNullOrWhiteSpace(OrganisationName) == false;
    }

    private bool HasMinimumEmployeeHeadcountIsValid()
    {
        return !IsLocalAuthority.HasValue || HasMinimumEmployeeHeadcount.HasValue;
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
        var reportingPercentages = GetPercentages("YourEmployees", "YourApprentices");
        var reportingPercentagesSchools = GetPercentages("SchoolsEmployees", "SchoolsApprentices");

        if (reportingPercentages != null && ReportingPercentages != null)
        {
            reportingPercentages.Title = ReportingPercentages.Title;
        }

        if (reportingPercentagesSchools != null && ReportingPercentagesSchools != null)
        {
            reportingPercentagesSchools.Title = ReportingPercentagesSchools.Title;
        }

        ReportingPercentages = reportingPercentages;
        ReportingPercentagesSchools = reportingPercentagesSchools;
    }

    private ReportingPercentages GetPercentages(string employeesSection, string apprenticesSection)
    {
        var percentages = new ReportingPercentages();

        if (Sections == null)
        {
            return null;
        }

        var employeeQuestions = GetQuestionSection(employeesSection);

        if (employeeQuestions == null)
        {
            return null;
        }

        var apprenticeQuestions = GetQuestionSection(apprenticesSection);

        if (apprenticeQuestions == null)
        {
            return null;
        }


        decimal employmentPeriod = 0, apprenticePeriod = 0, employmentEnd = 0, apprenticeEnd = 0, employmentStart = 0;

        if (string.IsNullOrWhiteSpace(employeeQuestions.Questions.Single(w => w.Id == "newThisPeriod").Answer) == false)
        {
            employmentPeriod = decimal.Parse(employeeQuestions.Questions.Single(w => w.Id == "newThisPeriod").Answer);
        }

        if (string.IsNullOrWhiteSpace(apprenticeQuestions.Questions.Single(w => w.Id == "newThisPeriod").Answer) == false)
        {
            apprenticePeriod = decimal.Parse(apprenticeQuestions.Questions.Single(w => w.Id == "newThisPeriod").Answer);
        }

        if (string.IsNullOrWhiteSpace(employeeQuestions.Questions.Single(w => w.Id == "atEnd").Answer) == false)
        {
            employmentEnd = decimal.Parse(employeeQuestions.Questions.Single(w => w.Id == "atEnd").Answer);
        }

        if (string.IsNullOrWhiteSpace(apprenticeQuestions.Questions.Single(w => w.Id == "atEnd").Answer) == false)
        {
            apprenticeEnd = decimal.Parse(apprenticeQuestions.Questions.Single(w => w.Id == "atEnd").Answer);
        }

        if (string.IsNullOrWhiteSpace(employeeQuestions.Questions.Single(w => w.Id == "atStart").Answer) == false)
        {
            employmentStart = decimal.Parse(employeeQuestions.Questions.Single(w => w.Id == "atStart").Answer);
        }

        if (apprenticePeriod != 0 & employmentPeriod != 0)
        {
            percentages.EmploymentStarts = (apprenticePeriod / employmentPeriod * 100).ToString("F2");
        }

        if (apprenticeEnd != 0 & employmentEnd != 0)
        {
            percentages.TotalHeadCount = (apprenticeEnd / employmentEnd * 100).ToString("F2");
        }

        if (apprenticePeriod != 0 & employmentStart != 0)
        {
            percentages.NewThisPeriod = (apprenticePeriod / employmentStart * 100).ToString("F2");
        }

        return percentages;
    }

    private IEnumerable<Section> GetSections()
    {
        var sectionList = new List<Section>();

        if (Sections == null)
        {
            return sectionList;
        }

        foreach (var reportSection in Sections)
        {
            sectionList.AddRange(GetSections(reportSection));
        }

        return sectionList;
    }

    private static IEnumerable<Section> GetSections(Section section)
    {
        var sectionList = new List<Section> { section };

        if (section.SubSections == null)
        {
            return sectionList;
        }

        foreach (var reportSection in section.SubSections)
        {
            sectionList.AddRange(GetSections(reportSection));
        }

        return sectionList;
    }

    public IEnumerable<string> GetNamesOfIncompleteMandatoryQuestionSections()
    {
        if (OrganisationNameIsValid() == false)
        {
            yield return "Organisation";
        }

        if (IsLocalAuthority.HasValue)
        {
            if (HasMinimumEmployeeHeadcountIsValid() == false)
            {
                yield return "TotalEmployees";
            }
        }

        foreach (var text in GetSummaryTextFromFirstLevelSubSectionsNotValidForSubmission())
        {
            yield return text;
        }
    }

    private IEnumerable<string> GetSummaryTextFromFirstLevelSubSectionsNotValidForSubmission()
    {
        return Sections.SelectMany(s => s.SubSections)
            .Where(w => !w.IsValidForSubmission())
            .Select(subSection => subSection.SummaryText);
    }
}