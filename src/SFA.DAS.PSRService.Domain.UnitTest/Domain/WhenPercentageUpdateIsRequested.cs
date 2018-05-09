using System;
using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;

namespace SFA.DAS.PSRService.Application.UnitTests.Domain
{
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
            Assert.IsNull(_report.ReportingPercentages);
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
                Sections = new[]
                {
                    new Section
                    {
                        Id = "YourApprentices",
                        SubSections = new List<Section>()
                        {
                            new Section
                            {
                                Id = "SubSectionTwo",
                                Questions = new[]
                                {
                                    new Question()
                                    {
                                        Id = "atStart",
                                        Answer = "250",
                                        Type = QuestionType.Number,
                                        Optional = false
                                    },
                                    new Question()
                                    {
                                        Id = "atEnd",
                                        Answer = "300",
                                        Type = QuestionType.Number,
                                        Optional = false
                                    },
                                    new Question()
                                    {
                                        Id = "newThisPeriod",
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
                }
            };

            // Act/Assert
           Assert.Throws<InvalidOperationException>(() => _report.UpdatePercentages());
            
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
                Sections = new[]
                {
                    new Section
                    {
                        Id = "YourEmployees",
                        SubSections = new List<Section>()
                        {
                            new Section
                            {
                                Id = "SubSectionTwo",
                                Questions = new List<Question>()
                                {
                                    new Question()
                                    {
                                        Id = "atStart",
                                        Answer = "250",
                                        Type = QuestionType.Number,
                                        Optional = false
                                    },
                                    new Question()
                                    {
                                        Id = "atEnd",
                                        Answer = "300",
                                        Type = QuestionType.Number,
                                        Optional = false
                                    },
                                    new Question()
                                    {
                                        Id = "newThisPeriod",
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
                }
            };

         

            // Act/Assert
            Assert.Throws<InvalidOperationException>(() => _report.UpdatePercentages());
        }

        [Test]
        public void And_Employee_AtStart_Is_zero_Then_zeror()
        {
            //Arrange
            var apprenticeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "250",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "atEnd",
                    Answer = "300",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "newThisPeriod",
                    Answer = "50",
                    Type = QuestionType.Number,
                    Optional = false
                }

            };
            var employeeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "0",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "atEnd",
                    Answer = "300",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "newThisPeriod",
                    Answer = "50",
                    Type = QuestionType.Number,
                    Optional = false
                }

            };



            var yourEmployees = new Section()
            {
                Id = "YourEmployeesSection",
                SubSections = new List<Section>()
                {
                    new Section
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

            var yourApprentices = new Section()
            {
                Id = "YourApprenticeSection",
                SubSections = new List<Section>()
                {
                    new Section
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


            IList<Section> sections = new List<Section>();

            sections.Add(yourEmployees);
            sections.Add(yourApprentices);
            _report = new Report()
            {
                ReportingPeriod = "1617",
                Sections = sections,
                SubmittedDetails = new Submitted(),
                Submitted = true
            };

            // Act
            _report.UpdatePercentages();

            // Assert
            Assert.IsNotNull(_report.ReportingPercentages);
            Assert.AreEqual(_report.ReportingPercentages.NewThisPeriod, 0);
        }

        [Test]
        public void And_Employee_Atend_Is_zero_Then_zero()
        {
            //Arrange
            var apprenticeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "250",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "atEnd",
                    Answer = "300",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "newThisPeriod",
                    Answer = "50",
                    Type = QuestionType.Number,
                    Optional = false
                }

            };
            var employeeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "250",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "atEnd",
                    Answer = "0",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "newThisPeriod",
                    Answer = "50",
                    Type = QuestionType.Number,
                    Optional = false
                }

            };



            var yourEmployees = new Section()
            {
                Id = "YourEmployeesSection",
                SubSections = new List<Section>()
                {
                    new Section
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

            var yourApprentices = new Section()
            {
                Id = "YourApprenticesSection",
                SubSections = new List<Section>()
                {
                    new Section
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


            IList<Section> sections = new List<Section>();

            sections.Add(yourEmployees);
            sections.Add(yourApprentices);
            _report = new Report()
            {
                ReportingPeriod = "1617",
                Sections = sections,
                SubmittedDetails = new Submitted(),
                Submitted = true
            };


            // Act
            _report.UpdatePercentages();

            // Assert
            Assert.IsNotNull(_report.ReportingPercentages);
            Assert.AreEqual(_report.ReportingPercentages.TotalHeadCount, 0);
        }

        [Test]
        public void And_Employee_newPeriod_Is_zero_Then_zero()
        {
            var apprenticeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "250",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "atEnd",
                    Answer = "300",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "newThisPeriod",
                    Answer = "50",
                    Type = QuestionType.Number,
                    Optional = false
                }

            };
            var employeeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "100",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "atEnd",
                    Answer = "300",
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



            var yourEmployees = new Section()
            {
                Id = "YourEmployeesSection",
                SubSections = new List<Section>()
                {
                    new Section
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

            var yourApprentices = new Section()
            {
                Id = "YourApprenticesSection",
                SubSections = new List<Section>()
                {
                    new Section
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


            IList<Section> sections = new List<Section>();

            sections.Add(yourEmployees);
            sections.Add(yourApprentices);
            _report = new Report()
            {
                ReportingPeriod = "1617",
                Sections = sections,
                SubmittedDetails = new Submitted(),
                Submitted = true
            };

            // Act
            _report.UpdatePercentages();

            // Assert
            Assert.IsNotNull(_report.ReportingPercentages);
            Assert.AreEqual(_report.ReportingPercentages.EmploymentStarts, 0);
        }

        [Test]
        public void And_Apprentice_Atend_Is_zero_Then_zero()
        {
            var apprenticeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "250",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "atEnd",
                    Answer = "0",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "newThisPeriod",
                    Answer = "50",
                    Type = QuestionType.Number,
                    Optional = false
                }

            };
            var employeeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "100",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "atEnd",
                    Answer = "300",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "newThisPeriod",
                    Answer = "50",
                    Type = QuestionType.Number,
                    Optional = false
                }

            };



            var yourEmployees = new Section()
            {
                Id = "YourEmployeesSection",
                SubSections = new List<Section>()
                {
                    new Section
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

            var yourApprentices = new Section()
            {
                Id = "YourApprenticesSection",
                SubSections = new List<Section>()
                {
                    new Section
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


            IList<Section> sections = new List<Section>();

            sections.Add(yourEmployees);
            sections.Add(yourApprentices);
            _report = new Report()
            {
                ReportingPeriod = "1617",
                Sections = sections,
                SubmittedDetails = new Submitted(),
                Submitted = true
            };

            // Act
            _report.UpdatePercentages();

            // Assert
            Assert.IsNotNull(_report.ReportingPercentages);
            Assert.AreEqual(_report.ReportingPercentages.TotalHeadCount, 0);
        }

        [Test]
        public void And_Apprentice_newPeriod_Is_zero_Then_zero()
        {
            var apprenticeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "250",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "atEnd",
                    Answer = "300",
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
            var employeeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "100",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "atEnd",
                    Answer = "300",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "newThisPeriod",
                    Answer = "50",
                    Type = QuestionType.Number,
                    Optional = false
                }

            };



            var yourEmployees = new Section()
            {
                Id = "YourEmployeesSection",
                SubSections = new List<Section>()
                {
                    new Section
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

            var yourApprentices = new Section()
            {
                Id = "YourApprenticesSection",
                SubSections = new List<Section>()
                {
                    new Section
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


            IList<Section> sections = new List<Section>();

            sections.Add(yourEmployees);
            sections.Add(yourApprentices);
            _report = new Report()
            {
                ReportingPeriod = "1617",
                Sections = sections,
                SubmittedDetails = new Submitted(),
                Submitted = true
            };

            // Act
            _report.UpdatePercentages();

            // Assert
            Assert.IsNotNull(_report.ReportingPercentages);
            Assert.AreEqual(_report.ReportingPercentages.EmploymentStarts, 0);
            Assert.AreEqual(_report.ReportingPercentages.NewThisPeriod, 0);
        }

        [Test]
        public void And_required_Answers_Are_Answered_Then_Percentages_Calculated()
        {
            //Arrange
            var apprenticeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "20",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "atEnd",
                    Answer = "35",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "newThisPeriod",
                    Answer = "18",
                    Type = QuestionType.Number,
                    Optional = false
                }

            };
            var employeeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "250",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "atEnd",
                    Answer = "300",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "newThisPeriod",
                    Answer = "50",
                    Type = QuestionType.Number,
                    Optional = false
                }

            };



            var yourEmployees = new Section()
            {
                Id = "YourEmployeesSection",
                SubSections = new List<Section>()
                {
                    new Section
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

            var yourApprentices = new Section()
            {
                Id = "YourApprenticeSection",
                SubSections = new List<Section>()
                {
                    new Section
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


            IList<Section> sections = new List<Section>();

            sections.Add(yourEmployees);
            sections.Add(yourApprentices);
            _report = new Report()
            {
                ReportingPeriod = "1617",
                Sections = sections,
                SubmittedDetails = new Submitted(),
                Submitted = true
            };

            // Act
            _report.UpdatePercentages();

            // Assert
            Assert.IsNotNull(_report.ReportingPercentages);
            Assert.AreEqual(_report.ReportingPercentages.TotalHeadCount.ToString("0.00"), "11.67");
            Assert.AreEqual(_report.ReportingPercentages.EmploymentStarts.ToString("0.00"), "36.00");
            Assert.AreEqual(_report.ReportingPercentages.NewThisPeriod.ToString("0.00"), "7.20");
        }
    }
}
