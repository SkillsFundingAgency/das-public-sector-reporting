using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SFA.DAS.PSRService.Domain;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Amend.Given_A_Submitted_Report;

[ExcludeFromCodeCoverage]
public abstract class GivenASubmittedReport : GivenAReportController
{
    private Report _currentReport;

    protected override void Given()
    {
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

        _currentReport = new Report
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
            It.IsAny<object>()));
        Controller.ObjectValidator = objectValidator.Object;

        MockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(_currentReport);
        MockReportService.Setup(s => s.CanBeEdited(_currentReport)).Returns(true);
    }
}