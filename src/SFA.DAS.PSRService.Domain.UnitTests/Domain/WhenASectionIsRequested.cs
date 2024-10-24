using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;

namespace SFA.DAS.PSRService.Domain.UnitTests.Domain;

[TestFixture]
public class WhenASectionIsRequested
{
    [Test]
    public void And_The_Question_Section_Exists_Then_Return_Section()
    {
        // arrange
        var questions = new List<Question>
        {
            new()
            {
                Id = QuestionIdentities.AtStart,
                Answer = "0",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = QuestionIdentities.AtEnd,
                Answer = "0",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = QuestionIdentities.NewThisPeriod,
                Answer = "0",
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

        var sections = new List<Section>
        {
            sectionOne,
            sectionTwo,
            sectionThree
        };

        var report = new Report
        {
            Sections = sections
        };

        // act
        var result = report.GetQuestionSection("SubSectionTwo");

        result.Should().NotBeNull();

        result.Id.Should().Be("SubSectionTwo");
        result.Title.Should().Be("SubSectionTwo");
        result.Questions.Should().NotBeNull();
        result.Questions.Should().NotBeEmpty();
    }

    [Test]
    public void And_The_Report_Contains_No_Sections_Then_Null_Returned()
    {
        new Report().GetQuestionSection("SubSectionTwo").Should().BeNull();
    }

    [Test]
    public void And_The_Question_Section_Exists_More_Than_Once_Then_Throw_Error()
    {
        // arrange
        var questions = new List<Question>
        {
            new()
            {
                Id = QuestionIdentities.AtStart,
                Answer = "0",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = QuestionIdentities.AtEnd,
                Answer = "0",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = QuestionIdentities.NewThisPeriod,
                Answer = "0",
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
                    Id = "SubSectionOne",
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
            Id = "SectionOne",
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

        var sections = new List<Section>
        {
            sectionOne,
            sectionTwo,
            sectionThree
        };

        var report = new Report
        {
            Sections = sections
        };

        // act
        var action = () => report.GetQuestionSection("SectionOne");

        // assert
        action.Should().Throw<InvalidOperationException>();
    }
}