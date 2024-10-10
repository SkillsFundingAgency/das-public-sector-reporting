using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Domain.UnitTests.Domain;

[TestFixture]
public class Given_A_Section_Exists
{
    [Test]
    public void When_There_Are_No_Questions_Then_It_Is_Complete_And_Is_Ready_For_Submission()
    {
        // arrange
        var section = new Section();

        // act
        // assert
        section.IsValidForSubmission().Should().BeTrue();
        section.IsComplete().Should().BeTrue();
    }

    [Test]
    public void When_There_Are_Optional_Unanswered_Questions_Then_It_Is_Not_Complete_And_Is_Ready_For_Submission()
    {
        // arrange
        var section = new Section
        {
            Questions =
            [
                new Question { Optional = true },
                new Question { Optional = true }
            ]
        };

        // act
        // assert
        section.IsValidForSubmission().Should().BeTrue();
        section.IsComplete().Should().BeFalse();
    }

    [Test]
    public void When_There_Are_Optional_Answered_Questions_Then_It_Is_Complete_And_Is_Ready_For_Submission()
    {
        // arrange
        var section = new Section
        {
            SubSections =
            [
                new Section
                {
                    Questions =
                    [
                        new Question { Answer = "somefing", Optional = true },
                        new Question { Answer = "somefing more", Optional = true }
                    ]
                }
            ]
        };

        // act
        // assert
        section.IsValidForSubmission().Should().BeTrue();
        section.IsComplete().Should().BeTrue();;
    }

    [Test]
    public void When_There_Are_Optional_Answered_And_Unanswered_Questions_Then_It_Is_Not_Complete_And_Is_Ready_For_Submission()
    {
        // arrange
        var section = new Section
        {
            Questions =
            [
                new Question { Optional = true },
                new Question { Answer = "somefing more", Optional = true }
            ],
            SubSections =
            [
                new Section
                {
                    Questions =
                    [
                        new Question { Optional = true },
                        new Question { Answer = "somefing more", Optional = true }
                    ]
                }
            ]
        };

        // act
        // assert
        section.IsValidForSubmission().Should().BeTrue();
        section.IsComplete().Should().BeFalse();
    }

    [Test]
    public void When_There_Are_Unanswered_Compulsory_Questions_Then_It_Is_Not_Complete_And_Not_Ready_For_Submission()
    {
        // arrange
        var section = new Section
        {
            Questions =
            [
                new Question { Optional = false },
                new Question { Optional = false }
            ],
            SubSections =
            [
                new Section
                {
                    Questions =
                    [
                        new Question { Optional = false },
                        new Question { Optional = false }
                    ]
                }
            ]
        };

        // act
        // assert
        section.IsValidForSubmission().Should().BeFalse();
        section.IsComplete().Should().BeFalse();
    }

    [Test]
    public void When_There_Are_Aanswered_Compulsory_Questions_Then_It_Is_Complete_And_Is_Ready_For_Submission()
    {
        // arrange
        var section = new Section
        {
            Questions =
            [
                new Question { Answer = "somefing", Optional = false },
                new Question { Answer = "somefing more", Optional = false }
            ],
            SubSections =
            [
                new Section
                {
                    Questions =
                    [
                        new Question { Answer = "somefing", Optional = false },
                        new Question { Answer = "somefing more", Optional = false }
                    ]
                }
            ]
        };

        // act
        // assert
        section.IsValidForSubmission().Should().BeTrue();
        section.IsComplete().Should().BeTrue();
    }

    [Test]
    public void When_There_Are_Unanswered_Compulsory_Questions_In_Subsection_Then_It_Is_Not_Complete_And_Not_Ready_For_Submission()
    {
        // arrange
        var section = new Section
        {
            Questions =
            [
                new Question { Answer = "somefing", Optional = false },
                new Question { Answer = "somefing more", Optional = false }
            ],
            SubSections =
            [
                new Section
                {
                    Questions =
                    [
                        new Question { Optional = false },
                        new Question { Optional = false }
                    ]
                }
            ]
        };

        // act
        // assert
        section.IsValidForSubmission().Should().BeFalse();
        section.IsComplete().Should().BeFalse();
    }
}