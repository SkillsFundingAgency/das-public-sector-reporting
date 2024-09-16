using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests;

[TestFixture]
public class Given_A_ReportController
{
    protected ReportController _controller;
    protected Mock<IReportService> _mockReportService;
    protected Mock<IUrlHelper> MockUrlHelper;
    private Mock<IEmployerAccountService> _employeeAccountServiceMock;
    public Mock<IUserService> _userServiceMock;
    private Mock<IPeriodService> _periodServiceMock;
    private EmployerIdentifier _employerIdentifier;
    protected Mock<IAuthorizationService> MockAuthorizationService;
    protected Mock<IMediator> MockMediatr;

    public Given_A_ReportController()
    {
        MockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
        _mockReportService = new Mock<IReportService>(MockBehavior.Strict);
        _employeeAccountServiceMock = new Mock<IEmployerAccountService>(MockBehavior.Strict);
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
        _periodServiceMock = new Mock<IPeriodService>(MockBehavior.Strict);

        MockAuthorizationService = new Mock<IAuthorizationService>();
            
        MockMediatr = new Mock<IMediator>();

        _periodServiceMock.Setup(s => s.GetCurrentPeriod()).Returns(Period.FromInstantInPeriod(DateTime.UtcNow));

        _controller = new ReportController(
            _mockReportService.Object, 
            _employeeAccountServiceMock.Object,
            _userServiceMock.Object, 
            null, 
            _periodServiceMock.Object,
            MockAuthorizationService.Object,
            MockMediatr.Object)
        {
            Url = MockUrlHelper.Object
        };

        _employerIdentifier = new EmployerIdentifier() {AccountId = "ABCDE", EmployerName = "EmployerName"};

        _employeeAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(It.IsAny<HttpContext>()))
            .Returns(_employerIdentifier);
        _employeeAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(null))
            .Returns(_employerIdentifier);

        _userServiceMock.Setup(s => s.GetUserModel(null)).Returns(new UserModel());
    }
    
    [OneTimeTearDown]
    public void TearDown() => _controller?.Dispose();

    [SetUp]
    public void GivenAndWhen()
    {
        Given();
        When();
    }

    protected virtual void When()
    {
    }

    protected virtual void Given()
    {
    }
}