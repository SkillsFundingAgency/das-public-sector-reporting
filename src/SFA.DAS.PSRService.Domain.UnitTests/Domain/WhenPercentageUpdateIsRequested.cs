using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;

namespace SFA.DAS.PSRService.Domain.UnitTests.Domain;

[TestFixture]
public class WhenPercentageUpdateIsRequested
{
    private Report _report;

    [Test]
    public void And_report_sections_Is_null_Then_error()
    {
        // Arrange
        _report = new Report();

        // Act
        _report.UpdatePercentages();

        // Assert
        _report.ReportingPercentages.Should().BeNull();
    }

    [Test]
    public void And_Employee_Section_Is_null_Then_error()
    {
        // Arrange
        _report = new Report
        {
            ReportingPeriod = "1617",
            SubmittedDetails = new Submitted(),
            Submitted = true,
            Sections =
            [
                new Section
                {
                    Id = "YourApprentices",
                    SubSections = new List<Section>
                    {
                        new()
                        {
                            Id = "SubSectionTwo",
                            Questions =
                            [
                                new Question
                                {
                                    Id = QuestionIdentities.AtStart,
                                    Answer = "250",
                                    Type = QuestionType.Number,
                                    Optional = false
                                },
                                new Question
                                {
                                    Id = QuestionIdentities.AtEnd,
                                    Answer = "300",
                                    Type = QuestionType.Number,
                                    Optional = false
                                },
                                new Question
                                {
                                    Id = QuestionIdentities.NewThisPeriod,
                                    Answer = "50",
                                    Type = QuestionType.Number,
                                    Optional = false
                                }
                            ],
                            Title = "SubSectionTwo",
                            SummaryText = ""
                        }
                    },
                    Questions = null,
                    Title = "SectionTwo"
                }
            ]
        };

        // Act
        _report.UpdatePercentages();

        // Assert
        _report.ReportingPercentages.Should().BeNull();
    }

    [Test]
    public void And_Apprentice_Section_Is_null_Then_error()
    {
        // Arrange
        _report = new Report
        {
            ReportingPeriod = "1617",
            SubmittedDetails = new Submitted(),
            Submitted = true,
            Sections =
            [
                new Section
                {
                    Id = "YourEmployees",
                    SubSections = new List<Section>
                    {
                        new()
                        {
                            Id = "SubSectionTwo",
                            Questions = new List<Question>
                            {
                                new()
                                {
                                    Id = QuestionIdentities.AtStart,
                                    Answer = "250",
                                    Type = QuestionType.Number,
                                    Optional = false
                                },
                                new()
                                {
                                    Id = QuestionIdentities.AtEnd,
                                    Answer = "300",
                                    Type = QuestionType.Number,
                                    Optional = false
                                },
                                new()
                                {
                                    Id = QuestionIdentities.NewThisPeriod,
                                    Answer = "50",
                                    Type = QuestionType.Number,
                                    Optional = false
                                }
                            },
                            Title = "SubSectionTwo",
                            SummaryText = ""
                        }
                    },
                    Questions = null,
                    Title = "SectionTwo"
                }
            ]
        };

        // Act
        _report.UpdatePercentages();

        // Assert
        _report.ReportingPercentages.Should().BeNull();
    }

    [Test]
    public void And_Employee_AtStart_Is_zero_Then_zeror()
    {
        //Arrange
        var apprenticeQuestions = new List<Question>
        {
            new()
            {
                Id = QuestionIdentities.AtStart,
                Answer = "250",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = QuestionIdentities.AtEnd,
                Answer = "300",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = QuestionIdentities.NewThisPeriod,
                Answer = "50",
                Type = QuestionType.Number,
                Optional = false
            }
        };
        var employeeQuestions = new List<Question>
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
                Answer = "300",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = QuestionIdentities.NewThisPeriod,
                Answer = "50",
                Type = QuestionType.Number,
                Optional = false
            }
        };

        var yourEmployees = new Section
        {
            Id = "YourEmployeesSection",
            SubSections = new List<Section>
            {
                new()
                {
                    Id = "YourEmployees",
                    Questions = employeeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""
                }
            },
            Questions = null,
            Title = "SectionTwo"
        };

        var yourApprentices = new Section
        {
            Id = "YourApprenticeSection",
            SubSections = new List<Section>
            {
                new()
                {
                    Id = "YourApprentices",
                    Questions = apprenticeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""
                }
            },
            Questions = null,
            Title = "SectionTwo"
        };


        var sections = new List<Section>
        {
            yourEmployees,
            yourApprentices
        };

        _report = new Report
        {
            ReportingPeriod = "1617",
            Sections = sections,
            SubmittedDetails = new Submitted(),
            Submitted = true
        };

        // Act
        _report.UpdatePercentages();

        // Assert
        _report.ReportingPercentages.Should().NotBeNull();
        _report.ReportingPercentages.NewThisPeriod.Should().Be("0.00");
    }

    [Test]
    public void And_Employee_Attend_Is_zero_Then_zero()
    {
        //Arrange
        var apprenticeQuestions = new List<Question>
        {
            new()
            {
                Id = QuestionIdentities.AtStart,
                Answer = "250",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = QuestionIdentities.AtEnd,
                Answer = "300",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = QuestionIdentities.NewThisPeriod,
                Answer = "50",
                Type = QuestionType.Number,
                Optional = false
            }
        };
        var employeeQuestions = new List<Question>
        {
            new()
            {
                Id = QuestionIdentities.AtStart,
                Answer = "250",
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
                Answer = "50",
                Type = QuestionType.Number,
                Optional = false
            }
        };

        var yourEmployees = new Section
        {
            Id = "YourEmployeesSection",
            SubSections = new List<Section>
            {
                new()
                {
                    Id = "YourEmployees",
                    Questions = employeeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""
                }
            },
            Questions = null,
            Title = "SectionTwo"
        };

        var yourApprentices = new Section
        {
            Id = "YourApprenticesSection",
            SubSections = new List<Section>
            {
                new()
                {
                    Id = "YourApprentices",
                    Questions = apprenticeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""
                }
            },
            Questions = null,
            Title = "SectionTwo"
        };

        var sections = new List<Section>
        {
            yourEmployees,
            yourApprentices
        };

        _report = new Report
        {
            ReportingPeriod = "1617",
            Sections = sections,
            SubmittedDetails = new Submitted(),
            Submitted = true
        };


        // Act
        _report.UpdatePercentages();

        // Assert
        _report.ReportingPercentages.Should().NotBeNull();
        _report.ReportingPercentages.TotalHeadCount.Should().Be("0.00");
    }

    [Test]
    public void And_Employee_newPeriod_Is_zero_Then_zero()
    {
        var apprenticeQuestions = new List<Question>
        {
            new()
            {
                Id = QuestionIdentities.AtStart,
                Answer = "250",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = QuestionIdentities.AtEnd,
                Answer = "300",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = QuestionIdentities.NewThisPeriod,
                Answer = "50",
                Type = QuestionType.Number,
                Optional = false
            }
        };
        var employeeQuestions = new List<Question>
        {
            new()
            {
                Id = QuestionIdentities.AtStart,
                Answer = "100",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = QuestionIdentities.AtEnd,
                Answer = "300",
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

        var yourEmployees = new Section
        {
            Id = "YourEmployeesSection",
            SubSections = new List<Section>
            {
                new()
                {
                    Id = "YourEmployees",
                    Questions = employeeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""
                }
            },
            Questions = null,
            Title = "SectionTwo"
        };

        var yourApprentices = new Section
        {
            Id = "YourApprenticesSection",
            SubSections = new List<Section>
            {
                new()
                {
                    Id = "YourApprentices",
                    Questions = apprenticeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""
                }
            },
            Questions = null,
            Title = "SectionTwo"
        };


        var sections = new List<Section>
        {
            yourEmployees,
            yourApprentices
        };

        _report = new Report
        {
            ReportingPeriod = "1617",
            Sections = sections,
            SubmittedDetails = new Submitted(),
            Submitted = true
        };

        // Act
        _report.UpdatePercentages();

        // Assert
        _report.ReportingPercentages.Should().NotBeNull();
        _report.ReportingPercentages.EmploymentStarts.Should().Be("0.00");
    }

    [Test]
    public void And_Apprentice_Attend_Is_zero_Then_zero()
    {
        var apprenticeQuestions = new List<Question>
        {
            new()
            {
                Id = QuestionIdentities.AtStart,
                Answer = "250",
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
                Answer = "50",
                Type = QuestionType.Number,
                Optional = false
            }
        };

        var employeeQuestions = new List<Question>
        {
            new()
            {
                Id = QuestionIdentities.AtStart,
                Answer = "100",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = QuestionIdentities.AtEnd,
                Answer = "300",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = QuestionIdentities.NewThisPeriod,
                Answer = "50",
                Type = QuestionType.Number,
                Optional = false
            }
        };

        var yourEmployees = new Section
        {
            Id = "YourEmployeesSection",
            SubSections = new List<Section>
            {
                new()
                {
                    Id = "YourEmployees",
                    Questions = employeeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""
                }
            },
            Questions = null,
            Title = "SectionTwo"
        };

        var yourApprentices = new Section
        {
            Id = "YourApprenticesSection",
            SubSections = new List<Section>
            {
                new()
                {
                    Id = "YourApprentices",
                    Questions = apprenticeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""
                }
            },
            Questions = null,
            Title = "SectionTwo"
        };

        var sections = new List<Section>
        {
            yourEmployees,
            yourApprentices
        };

        _report = new Report
        {
            ReportingPeriod = "1617",
            Sections = sections,
            SubmittedDetails = new Submitted(),
            Submitted = true
        };

        // Act
        _report.UpdatePercentages();

        // Assert
        _report.ReportingPercentages.Should().NotBeNull();
        _report.ReportingPercentages.TotalHeadCount.Should().Be("0.00");
    }

    [Test]
    public void And_Apprentice_newPeriod_Is_zero_Then_zero()
    {
        var apprenticeQuestions = new List<Question>
        {
            new()
            {
                Id = QuestionIdentities.AtStart,
                Answer = "250",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = QuestionIdentities.AtEnd,
                Answer = "300",
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
        var employeeQuestions = new List<Question>
        {
            new()
            {
                Id = QuestionIdentities.AtStart,
                Answer = "100",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = QuestionIdentities.AtEnd,
                Answer = "300",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = QuestionIdentities.NewThisPeriod,
                Answer = "50",
                Type = QuestionType.Number,
                Optional = false
            }
        };

        var yourEmployees = new Section
        {
            Id = "YourEmployeesSection",
            SubSections = new List<Section>
            {
                new()
                {
                    Id = "YourEmployees",
                    Questions = employeeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""
                }
            },
            Questions = null,
            Title = "SectionTwo"
        };

        var yourApprentices = new Section
        {
            Id = "YourApprenticesSection",
            SubSections = new List<Section>
            {
                new()
                {
                    Id = "YourApprentices",
                    Questions = apprenticeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""
                }
            },
            Questions = null,
            Title = "SectionTwo"
        };


        var sections = new List<Section>
        {
            yourEmployees,
            yourApprentices
        };

        _report = new Report
        {
            ReportingPeriod = "1617",
            Sections = sections,
            SubmittedDetails = new Submitted(),
            Submitted = true
        };

        // Act
        _report.UpdatePercentages();

        // Assert
        _report.ReportingPercentages.Should().NotBeNull();
        _report.ReportingPercentages.EmploymentStarts.Should().Be("0.00");
        _report.ReportingPercentages.NewThisPeriod.Should().Be("0.00");
    }

    [Test]
    public void And_required_Answers_Are_Answered_Then_Percentages_Calculated()
    {
        //Arrange
        var apprenticeQuestions = new List<Question>
        {
            new()
            {
                Id = QuestionIdentities.AtStart,
                Answer = "20",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = QuestionIdentities.AtEnd,
                Answer = "35",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = QuestionIdentities.NewThisPeriod,
                Answer = "18",
                Type = QuestionType.Number,
                Optional = false
            }
        };
        
        var employeeQuestions = new List<Question>
        {
            new()
            {
                Id = QuestionIdentities.AtStart,
                Answer = "250",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = QuestionIdentities.AtEnd,
                Answer = "300",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = QuestionIdentities.NewThisPeriod,
                Answer = "50",
                Type = QuestionType.Number,
                Optional = false
            }
        };

        var yourEmployees = new Section
        {
            Id = "YourEmployeesSection",
            SubSections = new List<Section>
            {
                new()
                {
                    Id = "YourEmployees",
                    Questions = employeeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""
                }
            },
            Questions = null,
            Title = "SectionTwo"
        };

        var yourApprentices = new Section
        {
            Id = "YourApprenticeSection",
            SubSections = new List<Section>
            {
                new()
                {
                    Id = "YourApprentices",
                    Questions = apprenticeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""
                }
            },
            Questions = null,
            Title = "SectionTwo"
        };

        var sections = new List<Section>
        {
            yourEmployees,
            yourApprentices
        };

        _report = new Report
        {
            ReportingPeriod = "1617",
            Sections = sections,
            SubmittedDetails = new Submitted(),
            Submitted = true
        };

        // Act
        _report.UpdatePercentages();

        // Assert
        _report.ReportingPercentages.Should().NotBeNull();
        _report.ReportingPercentages.TotalHeadCount.Should().Be("11.67");
        _report.ReportingPercentages.EmploymentStarts.Should().Be("36.00");
        _report.ReportingPercentages.NewThisPeriod.Should().Be("7.20");
    }
}