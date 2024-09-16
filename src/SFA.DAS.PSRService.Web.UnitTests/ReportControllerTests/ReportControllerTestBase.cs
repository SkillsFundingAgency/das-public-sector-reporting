using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading.Tasks;
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

[ExcludeFromCodeCoverage]
public class ReportControllerTestBase
{
    protected ReportController _controller;
    protected Mock<IReportService> _mockReportService;
    protected Mock<IUrlHelper> _mockUrlHelper;
    private Mock<IEmployerAccountService> _employeeAccountServiceMock;
    public Mock<IUserService> _userServiceMock;
    private Mock<IPeriodService> _periodServiceMock;
    protected IList<Report> ReportList;
    private EmployerIdentifier _employerIdentifier;

    protected Report CurrentValidNotSubmittedReport;

    [SetUp]
    public void SetUp()
    {
        CurrentValidNotSubmittedReport =
            new ReportBuilder()
                .WithValidSections()
                .WithEmployerId("VWZXYX")
                .ForCurrentPeriod()
                .WhereReportIsNotAlreadySubmitted()
                .Build();

        ReportList = new List<Report>(3);

        ReportList.Add(CurrentValidNotSubmittedReport);

        ReportList.Add(
            new ReportBuilder()
                .WithValidSections()
                .WithEmployerId("ABCDEF")
                .ForCurrentPeriod()
                .WhereReportIsNotAlreadySubmitted()
                .Build());

        ReportList.Add(
            new ReportBuilder()
                .WithValidSections()
                .WithEmployerId("ABCDEF")
                .ForPeriod(Period.FromInstantInPeriod(DateTime.UtcNow.AddYears(-1)))
                .WhereReportIsNotAlreadySubmitted()
                .Build());

        _mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
        _mockReportService = new Mock<IReportService>(MockBehavior.Strict);
        _employeeAccountServiceMock = new Mock<IEmployerAccountService>(MockBehavior.Strict);
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
        _periodServiceMock = new Mock<IPeriodService>(MockBehavior.Strict);

        _periodServiceMock.Setup(s => s.GetCurrentPeriod()).Returns(Period.FromInstantInPeriod(DateTime.UtcNow));

        _controller = new ReportController(
            _mockReportService.Object,
            _employeeAccountServiceMock.Object,
            _userServiceMock.Object, 
            null, 
            _periodServiceMock.Object,
            BuildAlwaysSuccessMockAuthorizationService(),
            Mock.Of<IMediator>()
        )
        {
            Url = _mockUrlHelper.Object
        };

        _employerIdentifier = new EmployerIdentifier() { AccountId = "ABCDE", EmployerName = "EmployerName" };

        _employeeAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(It.IsAny<HttpContext>()))
            .Returns(_employerIdentifier);
        _employeeAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(null))
            .Returns(_employerIdentifier);

        _userServiceMock.Setup(s => s.GetUserModel(null)).Returns(new UserModel());
    }
    
    [TearDown]
    public void TearDown() => _controller?.Dispose();

    private static IAuthorizationService BuildAlwaysSuccessMockAuthorizationService()
    {
        var mock = new Mock<IAuthorizationService>();

        mock
            .Setup(
                s => s.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<object>(),
                    It.IsAny<string>()))
            .Returns(
                Task.FromResult(AuthorizationResult.Failed()));

        return
            mock
                .Object;
    }
}