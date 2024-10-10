using System.Collections.Generic;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests;

[ExcludeFromCodeCoverage]
public class ReportControllerTestBase
{
    protected ReportController Controller;
    protected Mock<IReportService> MockReportService;
    protected Mock<IUrlHelper> MockUrlHelper;
    private Mock<IEmployerAccountService> _employeeAccountServiceMock;
    private Mock<IPeriodService> _periodServiceMock;
    protected List<Report> ReportList;
    private EmployerIdentifier _employerIdentifier;

    protected Report CurrentValidNotSubmittedReport;

    [SetUp]
    public void SetUp()
    {
        CurrentValidNotSubmittedReport = new ReportBuilder()
            .WithValidSections()
            .WithEmployerId("VWZXYX")
            .ForCurrentPeriod()
            .WhereReportIsNotAlreadySubmitted()
            .Build();

        ReportList = new List<Report>(3);

        ReportList.Add(CurrentValidNotSubmittedReport);

        ReportList.Add(new ReportBuilder()
            .WithValidSections()
            .WithEmployerId("ABCDEF")
            .ForCurrentPeriod()
            .WhereReportIsNotAlreadySubmitted()
            .Build());

        ReportList.Add(new ReportBuilder()
            .WithValidSections()
            .WithEmployerId("ABCDEF")
            .ForPeriod(Period.FromInstantInPeriod(DateTime.UtcNow.AddYears(-1)))
            .WhereReportIsNotAlreadySubmitted()
            .Build());

        MockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
        MockReportService = new Mock<IReportService>(MockBehavior.Strict);
        _employeeAccountServiceMock = new Mock<IEmployerAccountService>(MockBehavior.Strict);
        _periodServiceMock = new Mock<IPeriodService>(MockBehavior.Strict);

        _periodServiceMock.Setup(s => s.GetCurrentPeriod()).Returns(Period.FromInstantInPeriod(DateTime.UtcNow));

        Controller = new ReportController(
            MockReportService.Object,
            _employeeAccountServiceMock.Object,
            null,
            _periodServiceMock.Object,
            BuildAlwaysSuccessMockAuthorizationService(),
            Mock.Of<IMediator>()
        )
        {
            Url = MockUrlHelper.Object
        };

        _employerIdentifier = new EmployerIdentifier { AccountId = "ABCDE", EmployerName = "EmployerName" };

        _employeeAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(It.IsAny<HttpContext>()))
            .Returns(_employerIdentifier);

        _employeeAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(null))
            .Returns(_employerIdentifier);
    }

    [TearDown]
    public void TearDown() => Controller?.Dispose();

    private static IAuthorizationService BuildAlwaysSuccessMockAuthorizationService()
    {
        var mock = new Mock<IAuthorizationService>();

        mock.Setup(s => s.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                It.IsAny<string>()))
            .Returns(Task.FromResult(AuthorizationResult.Failed()));

        return mock.Object;
    }
}