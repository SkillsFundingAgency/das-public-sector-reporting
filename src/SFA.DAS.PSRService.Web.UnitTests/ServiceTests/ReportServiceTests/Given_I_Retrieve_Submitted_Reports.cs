﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MediatR;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.ReportServiceTests;

[TestFixture]
public class Given_I_Retrieve_Submitted_Reports
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
        _reportService = new ReportService(_configMock.Object, _mediatorMock.Object, _periodServiceMock.Object);
    }

    [Test]
    public async Task And_The_Report_Exists_Then_Show_Summary_Page()
    {
        // arrange
        _mediatorMock.Setup(s => s.Send(It.IsAny<GetSubmittedRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Report>().AsEnumerable());

        // act
        var result = await _reportService.GetSubmittedReports("ABCDE");

        _mediatorMock.Verify(m => m.Send(It.IsAny<GetSubmittedRequest>(), new CancellationToken()));

        result.Should().AllBeOfType<Report>();
    }
}