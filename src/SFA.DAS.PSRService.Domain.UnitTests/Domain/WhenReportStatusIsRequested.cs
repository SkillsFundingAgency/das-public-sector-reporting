using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;

namespace SFA.DAS.PSRService.Domain.UnitTests.Domain
{
    [TestFixture]
    public class WhenReportStatusIsRequested
    {
        [Test]
        public void And_No_Questions_Answered_And_No_OrganisationName_Then_Return_Started()
        {
            // arrange
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
                    Answer = "",
                    Type = QuestionType.Number,
                    Optional = false
                }

            };

            var sectionOne = new Section()
            {
                Id = "SectionOne",
                SubSections = new List<Section>() { new Section{
                    Id = "SubSectionOne",
                    Questions = questions,
                    Title = "SubSectionOne",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionOne"
            };

            var sectionTwo = new Section()
            {
                Id = "SectionTwo",
                SubSections = new List<Section>() { new Section{
                    Id = "SubSectionTwo",
                    Questions = questions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionTwo"
            };

            var sectionThree = new Section()
            {
                Id = "SectionThree",
                SubSections = new List<Section>() { new Section{
                    Id = "SubSectionThree",
                    Questions = questions,
                    Title = "SubSectionThree",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionThree"
            };

            IList<Section> sections = new List<Section>();

            sections.Add(sectionOne);
            sections.Add(sectionTwo);
            sections.Add(sectionThree);
            var report = new Report()
            {
                Sections = sections
            };

            // act
            var result = report.Status;


            Assert.NotNull(result);


            Assert.AreEqual(result, ReportStatus.Started);

        }
        [Test]
        public void And_One_Or_More_Questions_Answered_And_No_OrganisationName_Then_Return_InProgress()
        {
            // arrange
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
                    Answer = "0",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "newThisPeriod",
                    Answer = "",
                    Type = QuestionType.Number,
                    Optional = false
                }

            };

            var sectionOne = new Section()
            {
                Id = "SectionOne",
                SubSections = new List<Section>() { new Section{
                    Id = "SubSectionOne",
                    Questions = questions,
                    Title = "SubSectionOne",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionOne"
            };

            var sectionTwo = new Section()
            {
                Id = "SectionTwo",
                SubSections = new List<Section>() { new Section{
                    Id = "SubSectionTwo",
                    Questions = questions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionTwo"
            };

            var sectionThree = new Section()
            {
                Id = "SectionThree",
                SubSections = new List<Section>() { new Section{
                    Id = "SubSectionThree",
                    Questions = questions,
                    Title = "SubSectionThree",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionThree"
            };

            IList<Section> sections = new List<Section>();

            sections.Add(sectionOne);
            sections.Add(sectionTwo);
            sections.Add(sectionThree);
            var report = new Report()
            {
                Sections = sections
            };

            // act
            var result = report.Status;


            Assert.NotNull(result);


            Assert.AreEqual(result, ReportStatus.InProgress);

        }
        [Test]
        public void And_No_Questions_Answered_And_OrganisationName_Entered_Then_Return_InProgress()
        {
            // arrange
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
                    Answer = "",
                    Type = QuestionType.Number,
                    Optional = false
                }

            };

            var sectionOne = new Section()
            {
                Id = "SectionOne",
                SubSections = new List<Section>() { new Section{
                    Id = "SubSectionOne",
                    Questions = questions,
                    Title = "SubSectionOne",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionOne"
            };

            var sectionTwo = new Section()
            {
                Id = "SectionTwo",
                SubSections = new List<Section>() { new Section{
                    Id = "SubSectionTwo",
                    Questions = questions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionTwo"
            };

            var sectionThree = new Section()
            {
                Id = "SectionThree",
                SubSections = new List<Section>() { new Section{
                    Id = "SubSectionThree",
                    Questions = questions,
                    Title = "SubSectionThree",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionThree"
            };

            IList<Section> sections = new List<Section>();

            sections.Add(sectionOne);
            sections.Add(sectionTwo);
            sections.Add(sectionThree);
            var report = new Report()
            {
                OrganisationName = "Organisation",
                Sections = sections
            };

            // act
            var result = report.Status;


            Assert.NotNull(result);


            Assert.AreEqual(result, ReportStatus.InProgress);

        }
        [Test]
        public void And_Submitted_Then_Return_Completed()
        {
            // arrange
            var questions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "atEnd",
                    Answer = "",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "newThisPeriod",
                    Answer = "",
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

            var sectionTwo = new Section()
            {
                Id = "SectionTwo",
                SubSections = new List<Section>()
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

            var sectionThree = new Section()
            {
                Id = "SectionThree",
                SubSections = new List<Section>()
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

            IList<Section> sections = new List<Section>();

            sections.Add(sectionOne);
            sections.Add(sectionTwo);
            sections.Add(sectionThree);
            var report = new Report()
            {
                Sections = sections,
                Submitted = true
            };

            // act
            var result = report.Status;


            Assert.NotNull(result);


            Assert.AreEqual(result, ReportStatus.Completed);
        }
    }
}
