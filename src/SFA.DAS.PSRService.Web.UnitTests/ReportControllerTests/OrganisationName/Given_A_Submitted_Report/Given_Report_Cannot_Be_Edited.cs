﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.OrganisationName.Given_A_Submitted_Report;

public abstract class Given_Report_Cannot_Be_Edited : GivenWhenThen<ReportController>
{
    private Mock<IReportService> _mockReportService;
    private Mock<IUrlHelper> _mockUrlHelper;
    private Mock<IEmployerAccountService> _employeeAccountServiceMock;
    private Mock<IPeriodService> _periodServiceMock;
    private EmployerIdentifier _employerIdentifier;
    private Mock<IAuthorizationService> MockAuthorizationService;
    protected const string ExpectedUrl = "Home/Index";

    protected override void Given()
    {
        base.Given();

        _mockUrlHelper = new Mock<IUrlHelper>();
        _mockReportService = new Mock<IReportService>();
        _employeeAccountServiceMock = new Mock<IEmployerAccountService>();
        _periodServiceMock = new Mock<IPeriodService>();

        MockAuthorizationService = new Mock<IAuthorizationService>();

        _periodServiceMock.Setup(s => s.GetCurrentPeriod()).Returns(Period.FromInstantInPeriod(DateTime.UtcNow));

        _mockReportService.Setup(m => m.GetReport(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new Report { Submitted = true });

        _mockReportService.Setup(m => m.CanBeEdited(It.IsAny<Report>()))
            .Returns(false);

        _mockUrlHelper.Setup(m => m.Action(It.Is<UrlActionContext>(
                ctx =>
                    ctx.Action.Equals("Index", StringComparison.OrdinalIgnoreCase)
                    && ctx.Controller.Equals("Home", StringComparison.OrdinalIgnoreCase))))
            .Returns(ExpectedUrl);

        Sut = new ReportController(
            _mockReportService.Object,
            _employeeAccountServiceMock.Object,
            null,
            _periodServiceMock.Object,
            MockAuthorizationService.Object,
            Mock.Of<IMediator>())
        {
            Url = _mockUrlHelper.Object
        };

        _employerIdentifier = new EmployerIdentifier { AccountId = "ABCDE", EmployerName = "EmployerName" };

        _employeeAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(It.IsAny<HttpContext>()))
            .Returns(_employerIdentifier);
        
        _employeeAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(null))
            .Returns(_employerIdentifier);
    }
}