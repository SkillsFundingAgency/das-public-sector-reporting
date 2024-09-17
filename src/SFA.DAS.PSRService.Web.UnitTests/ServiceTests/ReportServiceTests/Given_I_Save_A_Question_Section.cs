using System;
using System.Collections.Generic;
using System.Threading;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.ReportServiceTests;

[TestFixture]
public class GivenISaveAQuestionSection
{
    private ReportService _reportService;
    private Mock<IMediator> _mediatorMock;
    private Mock<IWebConfiguration> _webConfigurationMock;
    private Mock<IPeriodService> _periodServiceMock;

    [SetUp]
    public void SetUp()
    {
        _mediatorMock = new Mock<IMediator>();
        _webConfigurationMock = new Mock<IWebConfiguration>(MockBehavior.Strict);
        _periodServiceMock = new Mock<IPeriodService>(MockBehavior.Strict);
        _reportService = new ReportService(_webConfigurationMock.Object, _mediatorMock.Object, _periodServiceMock.Object);
    }

    [Test]
    public void And_Section_And_Report_Supplied_Then_Create_Report()
    {
        //Arrange
        _webConfigurationMock.SetupGet(s => s.AuditWindowSize).Returns((TimeSpan?)null);
        _mediatorMock.Setup(s => s.Send(It.IsAny<UpdateReportRequest>(), It.IsAny<CancellationToken>()));

        var questions = new List<Question>
        {
            new()
            {
                Id = "atStart",
                Answer = "0",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = "atEnd",
                Answer = "0",
                Type = QuestionType.Number,
                Optional = false
            },
            new()
            {
                Id = "newThisPeriod",
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

        var user = new UserModel
        {
            DisplayName = "Horatio",
            Id = new Guid("DC850E8E-8286-47DF-8BFD-8332A6483555")
        };

        //Act
        _reportService.SaveReport(report, user, null);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateReportRequest>(), new CancellationToken()), Times.Once);
    }
}