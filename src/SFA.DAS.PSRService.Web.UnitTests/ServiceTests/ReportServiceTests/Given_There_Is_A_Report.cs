using System;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.ReportServiceTests
{
    [TestFixture]
    public class Given_ReportService_Is_Asked_For_Report_Editability
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<IWebConfiguration> _configMock;
        private Mock<IPeriodService> _periodServiceMock;
        private ReportService _reportService;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _configMock = new Mock<IWebConfiguration>(MockBehavior.Strict);
            _periodServiceMock = new Mock<IPeriodService>(MockBehavior.Strict);
            _reportService = new ReportService(_configMock.Object,_mediatorMock.Object, _periodServiceMock.Object);
        }
        
        [Test]
        public void When_There_Is_No_Report_Then_It_Cannot_Be_Edited()
        {
            Assert.IsFalse(_reportService.CanBeEdited(null));
        }
        
        [Test]
        public void When_It_Is_Not_Submitted_Then_It_Can_Be_Edited()
        {
            // arrange
            var report = new Report {Submitted = false, Period = new Period(DateTime.UtcNow)};
            _periodServiceMock.Setup(s => s.IsSubmissionsOpen()).Returns(true).Verifiable("IsSubmissionsOpen was never called");

            // act
            var result = _reportService.CanBeEdited(report);

            // assert
            _periodServiceMock.Verify(s => s.IsSubmissionsOpen(), Times.Once);
            Assert.IsTrue(result);
        }
        
        [Test]
        public void When_It_Is_Submitted_Then_It_Cannot_Be_Edited()
        {
            // arrange
            var report = new Report { Submitted = true, Period = new Period(DateTime.UtcNow) };
            _periodServiceMock.Setup(s => s.IsSubmissionsOpen()).Returns(true).Verifiable("IsSubmissionsOpen was never called");

            // act
            var result = _reportService.CanBeEdited(report);

            // assert
            _periodServiceMock.Verify(s => s.IsSubmissionsOpen(), Times.AtMostOnce);
            Assert.IsFalse(result);
        }
        
        [Test]
        public void When_It_Is_Not_Submitted_But_Period_Closed_Then_It_Cannot_Be_Edited()
        {
            // arrange
            var report = new Report { Submitted = false, Period = new Period(DateTime.UtcNow) };
            _periodServiceMock.Setup(s => s.IsSubmissionsOpen()).Returns(false).Verifiable("IsSubmissionsOpen was never called");

            // act
            var result = _reportService.CanBeEdited(report);

            // assert
            _periodServiceMock.Verify(s => s.IsSubmissionsOpen(), Times.Once);
            Assert.IsFalse(result);
        }        

        [Test]
        public void When_It_Is_Not_Submitted_But_For_Wrong_Period_Then_It_Cannot_Be_Edited()
        {
            // arrange
            var report = new Report { Submitted = false, Period = new Period(DateTime.UtcNow.AddYears(1)) };
            _periodServiceMock.Setup(s => s.IsSubmissionsOpen()).Returns(true);

            // act
            var result = _reportService.CanBeEdited(report);

            // assert
            _periodServiceMock.Verify(s => s.IsSubmissionsOpen(), Times.AtMostOnce);
            Assert.IsFalse(result);
        }
    }
}
