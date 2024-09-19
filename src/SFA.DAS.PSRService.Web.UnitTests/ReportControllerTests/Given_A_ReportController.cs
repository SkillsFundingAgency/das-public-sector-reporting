using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests;

public class GivenAReportController
{
    protected readonly ReportController Controller;
    protected readonly Mock<IReportService> MockReportService;
    protected readonly Mock<IUrlHelper> MockUrlHelper;
    private readonly Mock<IEmployerAccountService> _employeeAccountServiceMock;
    private readonly Mock<IPeriodService> _periodServiceMock;
    private readonly EmployerIdentifier _employerIdentifier;
    protected readonly Mock<IAuthorizationService> MockAuthorizationService;
    protected readonly Mock<IMediator> MockMediatr;

    public GivenAReportController()
    {
        MockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
        MockReportService = new Mock<IReportService>(MockBehavior.Strict);
        _employeeAccountServiceMock = new Mock<IEmployerAccountService>(MockBehavior.Strict);
        _periodServiceMock = new Mock<IPeriodService>(MockBehavior.Strict);

        MockAuthorizationService = new Mock<IAuthorizationService>();
            
        MockMediatr = new Mock<IMediator>();

        _periodServiceMock.Setup(s => s.GetCurrentPeriod()).Returns(Period.FromInstantInPeriod(DateTime.UtcNow));

        Controller = new ReportController(
            MockReportService.Object, 
            _employeeAccountServiceMock.Object,
            null, 
            _periodServiceMock.Object,
            MockAuthorizationService.Object,
            MockMediatr.Object)
        {
            Url = MockUrlHelper.Object
        };

        _employerIdentifier = new EmployerIdentifier {AccountId = "ABCDE", EmployerName = "EmployerName"};

        _employeeAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(It.IsAny<HttpContext>()))
            .Returns(_employerIdentifier);
        
        _employeeAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(null))
            .Returns(_employerIdentifier);
    }
    
    [OneTimeTearDown]
    public void TearDown() => Controller?.Dispose();

    [SetUp]
    public async Task GivenAndWhen()
    {
        Given();
        await When();
    }

    protected virtual Task When()
    {
        return Task.CompletedTask;
    }

    protected virtual void Given()
    {
    }
}