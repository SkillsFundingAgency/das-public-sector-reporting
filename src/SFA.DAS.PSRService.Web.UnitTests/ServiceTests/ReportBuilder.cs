using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests
{
    [ExcludeFromCodeCoverage]
    public sealed class ReportBuilder
    {
        private IEnumerable<Section> _sections = Enumerable.Empty<Section>();
        private bool _submittedStatus;

        public Report Build()
        {
            return
                new Report
                {
                    ReportingPeriod = "1718",
                    Sections = _sections,
                    Period = Period.ParsePeriodString("1718"),
                    SubmittedDetails = new Submitted(),
                    OrganisationName = "Some valid organisation name.",
                    Submitted = _submittedStatus
                };
        }

        public ReportBuilder WithValidSections()
        {
            _sections = BuildValidReportSections();

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

        private static IEnumerable<Section> BuildValidReportSections()
        {
            var questions = new List<Question>
            {
                new Question
                {
                    Id = "atStart",
                    Answer = "123",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question
                {
                    Id = "atEnd",
                    Answer = "123",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question
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
                    new Section
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
                    new Section
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
                    new Section
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

            List<Section> sections = new List<Section>(3);

            sections.Add(sectionOne);
            sections.Add(sectionTwo);
            sections.Add(sectionThree);

            return sections;
        }
    }
}