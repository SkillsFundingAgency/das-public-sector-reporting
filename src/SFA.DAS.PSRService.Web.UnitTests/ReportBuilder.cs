using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;

namespace SFA.DAS.PSRService.Web.UnitTests;

[ExcludeFromCodeCoverage]
public sealed class ReportBuilder
{
    private IEnumerable<Section> _sections = [];
    private bool _submittedStatus;
    private string _employerId = String.Empty;
    private Period _reportingPeriod = Period.ParsePeriodString("1718");

    public Report Build()
    {
        return
            new Report
            {
                ReportingPeriod = "1718",
                Sections = _sections,
                Period = _reportingPeriod,
                SubmittedDetails = new Submitted(),
                OrganisationName = $"Organisation {_employerId}",
                Submitted = _submittedStatus,
                EmployerId = _employerId
            };
    }

    public ReportBuilder WithValidSections()
    {
        _sections = BuildValidReportSections();

        return this;
    }

    public ReportBuilder WithInvalidSections()
    {
        _sections = BuildInvalidReportSections();

        return this;
    }

    public ReportBuilder WhereReportIsAlreadySubmitted()
    {
        _submittedStatus = true;

        return this;
    }

    public ReportBuilder WhereReportIsNotAlreadySubmitted()
    {
        _submittedStatus = false;

        return this;
    }


    public ReportBuilder WithEmployerId(string employerId)
    {
        _employerId = employerId;

        return this;
    }


    public ReportBuilder ForPeriod(Period period)
    {
        _reportingPeriod = period;

        return this;
    }


    public ReportBuilder ForCurrentPeriod()
    {
        _reportingPeriod = Period.FromInstantInPeriod(DateTime.UtcNow);

        return this;
    }

    private static IEnumerable<Section> BuildValidReportSections()
    {
        var questions = new List<Question>
        {
            new()
            {
                Id = "atStart",
                Answer = "123",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = "atEnd",
                Answer = "123",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = "newThisPeriod",
                Answer = "123",
                Type = QuestionType.Number,
                Optional = false
            }
        };

        var sectionOne = new Section
        {
            Id = "SectionOne",
            SubSections = new List<Section>
            {
                new()
                {
                    Id = "SubSectionOne",
                    Questions = questions,
                    Title = "SubSectionOne",
                    SummaryText = ""
                }
            },
            Questions = null,
            Title = "SectionOne"
        };

        var sectionTwo = new Section
        {
            Id = "SectionTwo",
            SubSections = new List<Section>
            {
                new()
                {
                    Id = "SubSectionTwo",
                    Questions = questions,
                    Title = "SubSectionTwo",
                    SummaryText = ""
                }
            },
            Questions = null,
            Title = "SectionTwo"
        };

        var sectionThree = new Section
        {
            Id = "SectionThree",
            SubSections = new List<Section>
            {
                new()
                {
                    Id = "SubSectionThree",
                    Questions = questions,
                    Title = "SubSectionThree",
                    SummaryText = ""
                }
            },
            Questions = null,
            Title = "SectionThree"
        };

        List<Section> sections =
        [
            sectionOne,
            sectionTwo,
            sectionThree
        ];

        return sections;
    }
    private static IEnumerable<Section> BuildInvalidReportSections()
    {
        var questions = new List<Question>()
        {
            new()
            {
                Id = "atStart",
                Answer = "",
                Type = QuestionType.Number,
                Optional = false
            }
            ,new()
            {
                Id = "atEnd",
                Answer = "",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = "newThisPeriod",
                Answer = "1,000",
                Type = QuestionType.Number,
                Optional = false
            }

        };

        var sectionOne = new Section()
        {
            Id = "SectionOne",
            SubSections = new List<Section>()
            {
                new()
                {
                    Id = "SubSectionOne",
                    Questions = questions,
                    Title = "SubSectionOne",
                    SummaryText = ""

                }
            },
            Questions = null,
            Title = "SectionOne"
        };

        return new List<Section>() { sectionOne };
    }
}