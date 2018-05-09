using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Services;
using Assert = NUnit.Framework.Assert;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests
{
    [TestFixture]
    public class Given_I_Create_A_Report
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

            _periodServiceMock.Setup(s => s.GetCurrentPeriod()).Returns(new Period(DateTime.UtcNow));

            _reportService = new ReportService(_webConfigurationMock.Object, _mediatorMock.Object, _periodServiceMock.Object);
            
        }

        [Test]
        public void And_Employer_Id_And_Period_Is_Supplied_Then_Create_Report()
        {
            //Arrange
            _webConfigurationMock.Setup(s => s.SubmissionClose).Returns(DateTime.UtcNow.AddDays(+3));
            _mediatorMock.Setup(s => s.Send(It.IsAny<CreateReportRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Report() {Id = Guid.NewGuid()});

            //Act

            _reportService.CreateReport("ABCDE");

            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateReportRequest>(), new CancellationToken()));

            //Assert
        }

        [Test]
        public void Employer_Id_And_Period_Is_Supplied_And_Submissions_Closed_Then_Throw_Exception()
        {
            //Arrange
            _webConfigurationMock.Setup(s => s.SubmissionClose).Returns(DateTime.UtcNow.AddDays(-3));
            _mediatorMock.Setup(s => s.Send(It.IsAny<CreateReportRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Report() { Id = Guid.NewGuid() });

            //Act

           Assert.Throws<Exception>(() => _reportService.CreateReport("ABCDE"));

        }

    }
}
