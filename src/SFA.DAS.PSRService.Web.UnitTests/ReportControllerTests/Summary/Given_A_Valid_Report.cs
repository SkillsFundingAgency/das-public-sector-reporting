using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Summary
{
    [ExcludeFromCodeCoverage]
    public class Given_A_Valid_Report
    :Given_A_ReportController
    {
        public Given_A_Valid_Report()
        {
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
            var report = new Report()
            {
                ReportingPeriod = "1617",
                Sections = sections,
                SubmittedDetails = new Submitted(),
                Submitted = true
            };

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));
            _controller.ObjectValidator = objectValidator.Object;

            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(report);
            _mockReportService.Setup(s => s.CanBeEdited(report)).Returns(true);
        }

        [ExcludeFromCodeCoverage]
        [TestFixture]
        public class When_Summary_Is_Called
            : Given_A_Valid_Report
        {
            private IActionResult result;

            public When_Summary_Is_Called()
            {
                result = _controller.Summary("1718");
            }

            [Test]
            public void Then_Result_Is_ViewResult()
            {
                Assert
                    .IsNotNull(result);

                Assert
                    .IsInstanceOf<ViewResult>(result);
            }

            [Test]
            public void Then_ViewName_Is_Summary()
            {
                Assert
                    .AreEqual("Summary", ((ViewResult)result).ViewName, "View name does not match, should be: Summary");
            }

            [Test]
            public void Then_ViewModel_Is_ReportViewModel()
            {
                Assert
                    .IsNotNull(((ViewResult)result).Model);

                Assert
                    .IsInstanceOf<ReportViewModel>(((ViewResult)result).Model);
            }

            [Test]
            public void Then_ViewModel_Has_Report()
            {
                var reportViewModel = ((ViewResult) result).Model as ReportViewModel;

                Assert
                    .IsNotNull(reportViewModel.Report);

                Assert
                    .IsNotNull(reportViewModel.Report.Id);
            }
        }
    }
}