using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests
{
    public abstract class Given_Home_Controller :GivenWhenThen<HomeController>
    {
        protected Mock<IReportService> _mockReportService;
        private Mock<IEmployerAccountService> _employeeAccountServiceMock;
        private EmployerIdentifier _employerIdentifier;
        private IWebConfiguration _webConfiguration;
        private Mock<IPeriodService> _mockPeriodService;
        protected Mock<IAuthorizationService> _authorizationServiceMock;
        protected string period = new Period(DateTime.UtcNow).PeriodString;

        protected override void Given()
        {
            _webConfiguration = new WebConfiguration();
            _webConfiguration.RootDomainUrl = "beetroot";

            _mockReportService = new Mock<IReportService>(MockBehavior.Strict);
            _employeeAccountServiceMock = new Mock<IEmployerAccountService>(MockBehavior.Strict);
            _mockPeriodService = new Mock<IPeriodService>();
            _authorizationServiceMock = new Mock<IAuthorizationService>(MockBehavior.Strict);

            _mockPeriodService.Setup(r => r.GetCurrentPeriod()).Returns(new Period(period));

            _employerIdentifier = new EmployerIdentifier() { AccountId = "ABCDE", EmployerName = "EmployerName" };
            _employeeAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(It.IsAny<HttpContext>()))
                .Returns(_employerIdentifier);
            _employeeAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(null))
                .Returns(_employerIdentifier);

            SUT = new HomeController(_mockReportService.Object, _employeeAccountServiceMock.Object, _webConfiguration, _mockPeriodService.Object, _authorizationServiceMock.Object);
            
        }
    }
}
