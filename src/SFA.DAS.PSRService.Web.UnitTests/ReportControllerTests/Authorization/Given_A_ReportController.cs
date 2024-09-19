using MediatR;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Authorization;

[ExcludeFromCodeCoverage]
public abstract class Given_A_ReportController : GivenWhenThen<ReportController>
{
    private Mock<IPeriodService> _periodServiceMock;
    private Mock<IAuthorizationService> _authorizationServiceMock;

    protected override void Given()
    {
        _authorizationServiceMock = new Mock<IAuthorizationService>(MockBehavior.Strict);
        _periodServiceMock = new Mock<IPeriodService>();

        _periodServiceMock.Setup(s => s.GetCurrentPeriod()).Returns(Period.FromInstantInPeriod(DateTime.UtcNow));

        Sut = new ReportController(null, null, null, _periodServiceMock.Object, _authorizationServiceMock.Object, Mock.Of<IMediator>());
    }
}