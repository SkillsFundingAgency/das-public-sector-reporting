using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Services;
using Assert = NUnit.Framework.Assert;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests
{
    [TestFixture]
    public class Given_I_Calculate_Report_Percentages
    {
        private ReportService _reportService;
        private Mock<IMediator> _mediatorMock;
        private Mock<IWebConfiguration> _webConfigurationMock;
        private Report report;
        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _webConfigurationMock = new Mock<IWebConfiguration>(MockBehavior.Strict);
            _reportService = new ReportService(_webConfigurationMock.Object, _mediatorMock.Object);

           


            _mediatorMock.Setup(s => s.Send(It.IsAny<GetReportRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(report);
        }

        [Test]
        public void And_report_Is_null_Then_error()
        {
            //Assert
            Assert.Throws<Exception>(() =>
                _reportService.CalculatePercentages(null));
        }

        [Test]
        public void And_report_sections_Is_null_Then_error()
        {
           
            //Assert
            Assert.Throws<Exception>(() =>
                _reportService.CalculatePercentages(new Report()));
        }

        [Test]
        public void And_Employee_Section_Is_null_Then_error()
        {
            //Arrange
            _webConfigurationMock.Setup(s => s.SubmissionClose).Returns(DateTime.UtcNow.AddDays(+3));
           
            var Questions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "250",
                    Type = QuestionType.Number,
                    Optional = false
                }
                ,new Question()
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

           

            var YourApprentices = new Section()
            {
                Id = "YourApprentices",
                SubSections = new List<Section>() { new Section{
                    Id = "SubSectionTwo",
                    Questions = Questions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionTwo"
            };



            IList<Section> sections = new List<Section>();
            
            sections.Add(YourApprentices);
            report = new Report()
            {
                ReportingPeriod = "1617",
                Sections = sections,
                SubmittedDetails = new Submitted(),
                Submitted = true
            };


            Assert.Throws<Exception>(() =>
                _reportService.CalculatePercentages(report));
            
            
        }
        [Test]
        public void And_Apprentice_Section_Is_null_Then_error()
        {
            //Arrange
            _webConfigurationMock.Setup(s => s.SubmissionClose).Returns(DateTime.UtcNow.AddDays(+3));

            var Questions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "250",
                    Type = QuestionType.Number,
                    Optional = false
                }
                ,new Question()
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



            var YourEmployees = new Section()
            {
                Id = "YourEmployees",
                SubSections = new List<Section>() { new Section{
                    Id = "SubSectionTwo",
                    Questions = Questions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionTwo"
            };



            IList<Section> sections = new List<Section>();

            sections.Add(YourEmployees);
            report = new Report()
            {
                ReportingPeriod = "1617",
                Sections = sections,
                SubmittedDetails = new Submitted(),
                Submitted = true
            };


            Assert.Throws<Exception>(() =>
                _reportService.CalculatePercentages(report));
        }

        [Test]
        public void And_Employee_AtStart_Is_zero_Then_zeror()
        {
            //Arrange
            _webConfigurationMock.Setup(s => s.SubmissionClose).Returns(DateTime.UtcNow.AddDays(+3));

            var ApprenticeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "250",
                    Type = QuestionType.Number,
                    Optional = false
                }
                ,new Question()
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
            var EmployeeQuestions = new List<Question>()
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



            var YourEmployees = new Section()
            {
                Id = "YourEmployeesSection",
                SubSections = new List<Section>() { new Section{
                    Id = "YourEmployees",
                    Questions = EmployeeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionTwo"
            };

            var YourApprentices = new Section()
            {
                Id = "YourApprenticeSection",
                SubSections = new List<Section>() { new Section{
                    Id = "YourApprentices",
                    Questions = ApprenticeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionTwo"
            };


            IList<Section> sections = new List<Section>();

            sections.Add(YourEmployees);
            sections.Add(YourApprentices);
            report = new Report()
            {
                ReportingPeriod = "1617",
                Sections = sections,
                SubmittedDetails = new Submitted(),
                Submitted = true
            };


            var result = _reportService.CalculatePercentages(report);

            Assert.AreEqual(result.NewThisPeriod, 0);
        }

        [Test]
        public void And_Employee_Atend_Is_zero_Then_zero()
        {
            //Arrange
            _webConfigurationMock.Setup(s => s.SubmissionClose).Returns(DateTime.UtcNow.AddDays(+3));

            var ApprenticeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "250",
                    Type = QuestionType.Number,
                    Optional = false
                }
                ,new Question()
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
            var EmployeeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "250",
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
                    Answer = "50",
                    Type = QuestionType.Number,
                    Optional = false
                }

            };



            var YourEmployees = new Section()
            {
                Id = "YourEmployeesSection",
                SubSections = new List<Section>() { new Section{
                    Id = "YourEmployees",
                    Questions = EmployeeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionTwo"
            };

            var YourApprentices = new Section()
            {
                Id = "YourApprenticesSection",
                SubSections = new List<Section>() { new Section{
                    Id = "YourApprentices",
                    Questions = ApprenticeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionTwo"
            };


            IList<Section> sections = new List<Section>();

            sections.Add(YourEmployees);
            sections.Add(YourApprentices);
            report = new Report()
            {
                ReportingPeriod = "1617",
                Sections = sections,
                SubmittedDetails = new Submitted(),
                Submitted = true
            };

            var result = _reportService.CalculatePercentages(report);
            
            Assert.AreEqual(result.TotalHeadCount,0);
        }
        [Test]
        public void And_Employee_newPeriod_Is_zero_Then_zero()
        {
            var ApprenticeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "250",
                    Type = QuestionType.Number,
                    Optional = false
                }
                ,new Question()
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
            var EmployeeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "100",
                    Type = QuestionType.Number,
                    Optional = false
                }
                ,new Question()
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



            var YourEmployees = new Section()
            {
                Id = "YourEmployeesSection",
                SubSections = new List<Section>() { new Section{
                    Id = "YourEmployees",
                    Questions = EmployeeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionTwo"
            };

            var YourApprentices = new Section()
            {
                Id = "YourApprenticesSection",
                SubSections = new List<Section>() { new Section{
                    Id = "YourApprentices",
                    Questions = ApprenticeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionTwo"
            };


            IList<Section> sections = new List<Section>();

            sections.Add(YourEmployees);
            sections.Add(YourApprentices);
            report = new Report()
            {
                ReportingPeriod = "1617",
                Sections = sections,
                SubmittedDetails = new Submitted(),
                Submitted = true
            };


            var result = _reportService.CalculatePercentages(report);

            Assert.AreEqual(result.EmploymentStarts, 0);
        }

        [Test]
        public void And_Apprentice_Atend_Is_zero_Then_zero()
        {
            var ApprenticeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "250",
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
                    Answer = "50",
                    Type = QuestionType.Number,
                    Optional = false
                }

            };
            var EmployeeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "100",
                    Type = QuestionType.Number,
                    Optional = false
                }
                ,new Question()
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



            var YourEmployees = new Section()
            {
                Id = "YourEmployeesSection",
                SubSections = new List<Section>() { new Section{
                    Id = "YourEmployees",
                    Questions = EmployeeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionTwo"
            };

            var YourApprentices = new Section()
            {
                Id = "YourApprenticesSection",
                SubSections = new List<Section>() { new Section{
                    Id = "YourApprentices",
                    Questions = ApprenticeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionTwo"
            };


            IList<Section> sections = new List<Section>();

            sections.Add(YourEmployees);
            sections.Add(YourApprentices);
            report = new Report()
            {
                ReportingPeriod = "1617",
                Sections = sections,
                SubmittedDetails = new Submitted(),
                Submitted = true
            };


            var result = _reportService.CalculatePercentages(report);

            Assert.AreEqual(result.TotalHeadCount, 0);
        }
        [Test]
        public void And_Apprentice_newPeriod_Is_zero_Then_zero()
        {
            var ApprenticeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "250",
                    Type = QuestionType.Number,
                    Optional = false
                }
                ,new Question()
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
            var EmployeeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "100",
                    Type = QuestionType.Number,
                    Optional = false
                }
                ,new Question()
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



            var YourEmployees = new Section()
            {
                Id = "YourEmployeesSection",
                SubSections = new List<Section>() { new Section{
                    Id = "YourEmployees",
                    Questions = EmployeeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionTwo"
            };

            var YourApprentices = new Section()
            {
                Id = "YourApprenticesSection",
                SubSections = new List<Section>() { new Section{
                    Id = "YourApprentices",
                    Questions = ApprenticeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionTwo"
            };


            IList<Section> sections = new List<Section>();

            sections.Add(YourEmployees);
            sections.Add(YourApprentices);
            report = new Report()
            {
                ReportingPeriod = "1617",
                Sections = sections,
                SubmittedDetails = new Submitted(),
                Submitted = true
            };


            var result = _reportService.CalculatePercentages(report);

            Assert.AreEqual(result.EmploymentStarts, 0);
            Assert.AreEqual(result.NewThisPeriod, 0);
        }

        [Test]
        public void And_required_Answers_Are_Answered_Then_Percentages_Calculated()
        {
            //Arrange
            var ApprenticeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "20",
                    Type = QuestionType.Number,
                    Optional = false
                }
                ,new Question()
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
            var EmployeeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "250",
                    Type = QuestionType.Number,
                    Optional = false
                }
                ,new Question()
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



            var YourEmployees = new Section()
            {
                Id = "YourEmployeesSection",
                SubSections = new List<Section>() { new Section{
                    Id = "YourEmployees",
                    Questions = EmployeeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionTwo"
            };

            var YourApprentices = new Section()
            {
                Id = "YourApprenticeSection",
                SubSections = new List<Section>() { new Section{
                    Id = "YourApprentices",
                    Questions = ApprenticeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionTwo"
            };


            IList<Section> sections = new List<Section>();

            sections.Add(YourEmployees);
            sections.Add(YourApprentices);
            report = new Report()
            {
                ReportingPeriod = "1617",
                Sections = sections,
                SubmittedDetails = new Submitted(),
                Submitted = true
            };


            var result = _reportService.CalculatePercentages(report);

            Assert.AreEqual(result.TotalHeadCount.ToString("0.00"),"11.67");
            Assert.AreEqual(result.EmploymentStarts.ToString("0.00"), "36.00");
            Assert.AreEqual(result.NewThisPeriod.ToString("0.00"), "7.20");
        }
    }
}
