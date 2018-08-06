using System;
using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;

namespace SFA.DAS.PSRService.Application.UnitTests.Domain
{
    [TestFixture]
    public class WhenASectionIsRequested
    {
        [Test]
        public void And_The_Question_Section_Exists_Then_Return_Section()
        {
            // arrange
            var questions = new List<Question>()
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
                    Answer = "0",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "newThisPeriod",
                    Answer = "0",
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
            var result = report.GetQuestionSection("SubSectionTwo");

        
            Assert.NotNull(result);
         

            Assert.AreEqual(result.Id, "SubSectionTwo");
            Assert.AreEqual(result.Title, "SubSectionTwo");
            Assert.NotNull(result.Questions);
            CollectionAssert.IsNotEmpty(result.Questions);

        }


        [Test]
        public void And_The_Report_Contains_No_Sections_Then_Null_Returned()
        {
            Assert.IsNull(new Report().GetQuestionSection("SubSectionTwo"));
        }

        [Test]
        public void And_The_Question_Section_Exists_More_Than_Once_Then_Throw_Error()
        {
            // arrange
            var questions = new List<Question>()
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
                    Answer = "0",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "newThisPeriod",
                    Answer = "0",
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
                    Id = "SubSectionOne",
                    Questions = questions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionTwo"
            };

            var sectionThree = new Section()
            {
                Id = "SectionOne",
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

            Assert.Throws<InvalidOperationException>(() => report.GetQuestionSection("SectionOne"));


        }
    }
}
