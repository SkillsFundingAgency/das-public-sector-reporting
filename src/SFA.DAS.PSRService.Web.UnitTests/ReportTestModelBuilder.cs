using System;
using System.Collections.Generic;
using System.Text;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;

namespace SFA.DAS.PSRService.Web.UnitTests
{
    public static class ReportTestModelBuilder
    {
        private static readonly IList<Question>Questions = new List<Question>()
        {
            new Question()
            {
                Id = "atStart",
                Answer = "0",
                Type = QuestionType.Number,
                Optional = false
            }
            ,new Question()
            {
                Id = "atEnd",
                Answer = "1000",
                Type = QuestionType.Number,
                Optional = false
            },
            new Question()
            {
                Id = "newThisPeriod",
                Answer = "1,000",
                Type = QuestionType.Number,
                Optional = false
            }

        };

        public static readonly Section SectionOne = new Section()
        {
            Id = "SectionOne",
            SubSections = new List<Section>()
            {
                new Section
                {
                    Id = "SubSectionOne",
                    Questions = Questions,
                    Title = "SubSectionOne",
                    SummaryText = ""

                }
            },
            Questions = null,
            Title = "SectionOne"
        };
        public static readonly Section SectionTwo = new Section()
        {
            Id = "SectionTwo",
            SubSections = new List<Section>()
            {
                new Section
                {
                    Id = "SubSectionTwo",
                    Questions = Questions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }
            },
            Questions = null,
            Title = "SectionTwo"
        };
        public static Report CurrentReportWithValidSections(string employerId)
        {
            return ReportWithValidSections(employerId, DateTime.UtcNow);
        }
        public static Report CurrentReportWithDuplicateSections(string employerId)
        {
            return ReportWithValidSections(employerId, DateTime.UtcNow);
        }
        public static Report ReportWithValidSections(string employerId, DateTime periodDateTime)
        {
            var period = new Period(periodDateTime);

            return new Report()
            {
                Id = Guid.NewGuid(),
                ReportingPeriod = period.PeriodString,
                Period = period,
                EmployerId = employerId,
                OrganisationName = $"Organisation {employerId}",
                Sections = GetValidSectionList()
            };
        }
        public static Report ReportWithDuplicateSections(string employerId, DateTime periodDateTime)
        {
            var period = new Period(periodDateTime);

            return new Report()
            {
                Id = Guid.NewGuid(),
                ReportingPeriod = period.PeriodString,
                Period = period,
                EmployerId = employerId,
                OrganisationName = $"Organisation {employerId}",
                Sections = GetDuplicateSectionList()
            };
        }

        private static IEnumerable<Section> GetValidSectionList()
        {
            

            return new List<Section>() { SectionOne };
        }
        private static IEnumerable<Section> GetDuplicateSectionList()
        {


            return new List<Section>() { SectionOne,SectionOne };
        }

        public static IList<Report> ReportsWithValidSections()
        {
            return new List<Report>()
            {
                CurrentReportWithValidSections("ABCDE"),
                ReportWithValidSections("ABCDE", DateTime.UtcNow.AddYears(-1)),
                CurrentReportWithValidSections("VWXYZ")
            };
        }

        public static Report CurrentReportWithInvalidSections(string employerId)
        {
            return ReportWithInvalidSections(employerId, DateTime.UtcNow);
        }
        public static Report ReportWithInvalidSections(string employerId, DateTime periodDateTime)
        {
            var period = new Period(periodDateTime);

            return new Report()
            {
                Id = Guid.NewGuid(),
                ReportingPeriod = period.PeriodString,
                Period = period,
                EmployerId = employerId,
                OrganisationName = $"Organisation {employerId}",
                Sections = GetInvalidSectionList()
            };
        }
        private static IEnumerable<Section> GetInvalidSectionList()
        {
            var questions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "",
                    Type = QuestionType.Number,
                    Optional = false
                }
                ,new Question()
                {
                    Id = "atEnd",
                    Answer = "",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
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

            return new List<Section>() { sectionOne };
        }

       
    }
}
