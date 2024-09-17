using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.ReportServiceTests;

[TestFixture]
public class Given_I_Calculate_A_Report_Period
{
    private ReportService _reportService;
    private Mock<IMediator> _mediatorMock;
    private Mock<IWebConfiguration> _webConfigurationMock;
    private Mock<IPeriodService> _periodServiceMock;

    [SetUp]
    public void Setup()
    {
        _mediatorMock = new Mock<IMediator>();
        _webConfigurationMock = new Mock<IWebConfiguration>(MockBehavior.Strict);
        _periodServiceMock = new Mock<IPeriodService>(MockBehavior.Strict);

        _reportService = new ReportService(_webConfigurationMock.Object, _mediatorMock.Object, _periodServiceMock.Object);
    }
}