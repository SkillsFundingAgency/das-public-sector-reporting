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
    public class Given_I_Submit_A_Report
    {
        private ReportService _reportService;
        private Mock<IMediator> _mediatorMock;
        private Mock<IWebConfiguration> _webConfigurationMock;
        private Mock<IPeriodService> _periodServiceMock;
        private Report report;
        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _webConfigurationMock = new Mock<IWebConfiguration>(MockBehavior.Strict);
            _periodServiceMock = new Mock<IPeriodService>(MockBehavior.Strict);

            _reportService = new ReportService(_webConfigurationMock.Object, _mediatorMock.Object, _periodServiceMock.Object);

            var Questions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "123",
                    Type = QuestionType.Number,
                    Optional = false
                }
                ,new Question()
                {
                    Id = "atEnd",
                    Answer = "123",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "newThisPeriod",
                    Answer = "123",
                    Type = QuestionType.Number,
                    Optional = false
                }

            };

            var SectionOne = new Section()
            {
                Id = "SectionOne",
                SubSections = new List<Section>() { new Section{
                    Id = "SubSectionOne",
                    Questions = Questions,
                    Title = "SubSectionOne",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionOne"
            };

            var SectionTwo = new Section()
            {
                Id = "SectionTwo",
                SubSections = new List<Section>() { new Section{
                    Id = "SubSectionTwo",
                    Questions = Questions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionTwo"
            };

            var SectionThree = new Section()
            {
                Id = "SectionThree",
                SubSections = new List<Section>() { new Section{
                    Id = "SubSectionThree",
                    Questions = Questions,
                    Title = "SubSectionThree",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionThree"
            };

            IList<Section> sections = new List<Section>();

            sections.Add(SectionOne);
            sections.Add(SectionTwo);
            sections.Add(SectionThree);
             report = new Report()
            {
                ReportingPeriod = "1718",
                Sections = sections,
                Period = new Period("1718")
            };

            report.SubmittedDetails = new Submitted();

            _mediatorMock.Setup(s => s.Send(It.IsAny<GetReportRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(report);
        }

        [Test]
        public void And_report_Is_Valid_Then_Submit_Report()
        {
            //Arrange
            _webConfigurationMock.Setup(s => s.SubmissionClose).Returns(DateTime.UtcNow.AddDays(+3));
            
 
            //Act
            var result =
                _reportService.SubmitReport(report.ReportingPeriod, report.EmployerId, report.SubmittedDetails);

            _mediatorMock.Verify(m => m.Send(It.IsAny<SubmitReportRequest>(), new CancellationToken()));

            //Assert
            Assert.AreEqual(SubmittedStatus.Submitted, result);
        }

        [Test]
        public void And_report_Is_Invalid_Then_return_Invalid()
        {
            //Arrange
            _webConfigurationMock.Setup(s => s.SubmissionClose).Returns(DateTime.UtcNow.AddDays(+3));
            report.Submitted = true;

            //Act
            var result =
                _reportService.SubmitReport(report.ReportingPeriod, report.EmployerId, report.SubmittedDetails);

           

            //Assert

            Assert.AreEqual(SubmittedStatus.Invalid, result);
        }


    }
}
