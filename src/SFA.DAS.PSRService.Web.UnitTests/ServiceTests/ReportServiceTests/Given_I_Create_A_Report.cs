using System;
using System.Threading;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;
using Assert = NUnit.Framework.Assert;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.ReportServiceTests;

[TestFixture]
public class Given_I_Create_A_Report
{
    private ReportService _reportService;
    private Mock<IMediator> _mediatorMock;
    private Mock<IWebConfiguration> _webConfigurationMock;
    private Mock<IPeriodService> _periodServiceMock;
    private Period _period;
    private UserModel _user;

    [SetUp]
    public void SetUp()
    {
        _mediatorMock = new Mock<IMediator>();
        _periodServiceMock = new Mock<IPeriodService>(MockBehavior.Strict);

        _period = Period.FromInstantInPeriod(DateTime.UtcNow);
        _periodServiceMock.Setup(s => s.GetCurrentPeriod()).Returns(_period);

        _reportService = new ReportService(Mock.Of<IWebConfiguration>(), _mediatorMock.Object, _periodServiceMock.Object);
        _user = new UserModel { Id = Guid.NewGuid(), DisplayName = "Vladimir" };
    }

    [TestCase(true)]
    [TestCase(false)]
    public void And_Employer_Id_And_Period_Is_Supplied_Then_Create_Report(bool isLocalAuthority)
    {
        //Arrange
        CreateReportRequest actualRequest = null;
        _mediatorMock.Setup(s => s.Send(It.IsAny<CreateReportRequest>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<Report>, CancellationToken>((r, c) => actualRequest = (CreateReportRequest)r)
            .ReturnsAsync(new Report { Id = Guid.NewGuid() })
            .Verifiable();

        //Act
        _reportService.CreateReport("ABCDE", _user, isLocalAuthority);

        //Assert
        _mediatorMock.VerifyAll();
        actualRequest.EmployerId.Should().Be("ABCDE");
        actualRequest.Period.Should().Be(_period.PeriodString);
        actualRequest.User.Name.Should().Be(_user.DisplayName);
        actualRequest.User.Id.Should().Be(_user.Id);
    }
}