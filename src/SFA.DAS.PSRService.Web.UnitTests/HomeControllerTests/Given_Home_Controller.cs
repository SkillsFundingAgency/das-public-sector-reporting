using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests;

public abstract class Given_Home_Controller :GivenWhenThen<HomeController>
{
    protected Mock<IReportService> MockReportService;
    protected Mock<IAuthorizationService> AuthorizationServiceMock;
    protected readonly Period CurrentPeriod = Period.FromInstantInPeriod(DateTime.UtcNow);
    
    private Mock<IEmployerAccountService> _employeeAccountServiceMock;
    private EmployerIdentifier _employerIdentifier;
    private IWebConfiguration _webConfiguration;
    private Mock<IPeriodService> _mockPeriodService;

    protected override void Given()
    {
        _webConfiguration = new WebConfiguration();
        _webConfiguration.RootDomainUrl = "beetroot";

        MockReportService = new Mock<IReportService>(MockBehavior.Strict);
        _employeeAccountServiceMock = new Mock<IEmployerAccountService>(MockBehavior.Strict);
        _mockPeriodService = new Mock<IPeriodService>();
        AuthorizationServiceMock = new Mock<IAuthorizationService>(MockBehavior.Strict);

        _mockPeriodService.Setup(r => r.GetCurrentPeriod()).Returns(CurrentPeriod);

        _employerIdentifier = new EmployerIdentifier { AccountId = "ABCDE", EmployerName = "EmployerName" };
        _employeeAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(It.IsAny<HttpContext>()))
            .Returns(_employerIdentifier);
        _employeeAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(null))
            .Returns(_employerIdentifier);

        Sut = new HomeController(MockReportService.Object, _employeeAccountServiceMock.Object, _webConfiguration, _mockPeriodService.Object, AuthorizationServiceMock.Object, null);
    }
}