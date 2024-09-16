using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Moq;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Amend.Given_A_Submitted_Report;

[ExcludeFromCodeCoverage]
public abstract class Given_A_Submitted_Report
    :Given_A_ReportController
{
    protected Report CurrentReport;

    protected override void Given()
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
        CurrentReport = new Report()
        {
            Id = Guid.NewGuid(),
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

        _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(CurrentReport);
        _mockReportService.Setup(s => s.CanBeEdited(CurrentReport)).Returns(true);
    }
}