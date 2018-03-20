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
using SFA.DAS.PSRService.Web.Services;
using Assert = NUnit.Framework.Assert;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests
{
    [TestFixture]
    public class Given_I_Create_A_Report
    {
        private ReportService _reportService;
        private Mock<IMediator> _mediatorMock;
        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _reportService = new ReportService(null, _mediatorMock.Object);
            
        }

        [Test]
        public void And_Employer_Id_And_Period_Is_Supplied_Then_Create_Report()
        {
            //Arrange

            _mediatorMock.Setup(s => s.Send(It.IsAny<CreateReportRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Report() {Id = Guid.NewGuid()});

            //Act

            _reportService.CreateReport(12345);

            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateReportRequest>(), new CancellationToken()));

            //Assert
        }


     
    }
}
