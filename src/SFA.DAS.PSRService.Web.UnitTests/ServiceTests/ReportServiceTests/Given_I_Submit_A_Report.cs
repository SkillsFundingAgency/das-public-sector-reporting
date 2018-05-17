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
using SFA.DAS.PSRService.Web.Services;
using Assert = NUnit.Framework.Assert;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.ReportServiceTests
{
    [TestFixture]
    public class Given_I_Submit_A_Report
    {
        private ReportService _reportService;
        private Mock<IMediator> _mediatorMock;
        private Mock<IWebConfiguration> _webConfigurationMock;
        private Mock<IPeriodService> _periodServiceMock;
        private Report _report;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _webConfigurationMock = new Mock<IWebConfiguration>(MockBehavior.Strict);
            _periodServiceMock = new Mock<IPeriodService>(MockBehavior.Strict);

            _reportService = new ReportService(_webConfigurationMock.Object, _mediatorMock.Object, _periodServiceMock.Object);

            var questions = new List<Question>
            {
                new Question
                {
                    Id = "atStart",
                    Answer = "123",
                    Type = QuestionType.Number,
                    Optional = false
                }
                ,new Question
                {
                    Id = "atEnd",
                    Answer = "123",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question
                {
                    Id = "newThisPeriod",
                    Answer = "123",
                    Type = QuestionType.Number,
                    Optional = false
                }

            };

            var sectionOne = new Section
            {
                Id = "SectionOne",
                SubSections = new List<Section> { new Section{
                    Id = "SubSectionOne",
                    Questions = questions,
                    Title = "SubSectionOne",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionOne"
            };

            var sectionTwo = new Section
            {
                Id = "SectionTwo",
                SubSections = new List<Section> { new Section{
                    Id = "SubSectionTwo",
                    Questions = questions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionTwo"
            };

            var sectionThree = new Section
            {
                Id = "SectionThree",
                SubSections = new List<Section> { new Section{
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

            _report = new Report
            {
                ReportingPeriod = "1718",
                Sections = sections,
                Period = new Period("1718"),
                SubmittedDetails = new Submitted()
            };


            _mediatorMock.Setup(s => s.Send(It.IsAny<GetReportRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_report);
        }

        [Test]
        public void And_report_Is_Valid_Then_Submit_Report()
        {
            //Arrange
            _webConfigurationMock.Setup(s => s.SubmissionClose).Returns(DateTime.UtcNow.AddDays(+3));
            _periodServiceMock.Setup(s => s.IsSubmissionsOpen()).Returns(true).Verifiable();
 
            //Act
            _reportService.SubmitReport(_report);

            //Assert
            _periodServiceMock.VerifyAll();
            _mediatorMock.Verify(m => m.Send(It.IsAny<SubmitReportRequest>(), new CancellationToken()));
        }

        [Test]
        public void And_report_Is_Invalid_Then_Throw_Exception()
        {
            //Arrange
            _webConfigurationMock.Setup(s => s.SubmissionClose).Returns(DateTime.UtcNow.AddDays(+3));
            _report.Submitted = true;

            //Act
            //Assert
            Assert.Throws<Exception>(() => _reportService.SubmitReport(_report));
        }
    }
}
