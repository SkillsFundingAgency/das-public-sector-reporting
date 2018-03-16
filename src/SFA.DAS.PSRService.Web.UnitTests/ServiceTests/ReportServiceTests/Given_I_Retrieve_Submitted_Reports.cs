using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests
{
    [TestFixture]
    public class Given_I_Retrieve_Submitted_Reports
    {
        public Mock<IMediator> _mediatorMock;
        public ReportService _reportService;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _reportService = new ReportService(_mediatorMock.Object);

        }
        
        [Test]
        public void And_The_Report_Exists_Then_Show_Summary_Page()
        {
            // arrange

            _mediatorMock.Setup(s => s.Send(It.IsAny<GetSubmittedRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Report>());
            // act
            var result = _reportService.GetSubmittedReports(12345);

            _mediatorMock.Verify(m => m.Send(It.IsAny<GetSubmittedRequest>(), new CancellationToken()));

            Assert.AreEqual(typeof(IEnumerable<Report>), result.GetType());

        }


    }
}
