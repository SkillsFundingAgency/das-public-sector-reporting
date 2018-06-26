using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Edit.Given_Report_Can_Be_Edited
{
    public abstract class Given_Report_Can_Be_Edited
    : GivenWhenThen<ReportController>
    {
        private Mock<IReportService> _mockReportService;
        private Mock<IUrlHelper> _mockUrlHelper;
        private Mock<IEmployerAccountService> _employeeAccountServiceMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<IPeriodService> _periodServiceMock;
        private EmployerIdentifier _employerIdentifier;
        protected Mock<IAuthorizationService> MockAuthorizationService;

        protected override void Given()
        {
            base.Given();


            _mockUrlHelper = new Mock<IUrlHelper>();
            _mockReportService = new Mock<IReportService>();
            _employeeAccountServiceMock = new Mock<IEmployerAccountService>();
            _userServiceMock = new Mock<IUserService>();
            _periodServiceMock = new Mock<IPeriodService>();

            MockAuthorizationService = new Mock<IAuthorizationService>();

            _periodServiceMock.Setup(s => s.GetCurrentPeriod()).Returns(new Period(DateTime.UtcNow));

            _mockReportService
                .Setup(
                    m =>
                        m.CanBeEdited(
                            It.IsAny<Report>()))
                .Returns(true);

            SUT = new ReportController(
                _mockReportService.Object,
                _employeeAccountServiceMock.Object,
                _userServiceMock.Object,
                null,
                _periodServiceMock.Object,
                MockAuthorizationService.Object)
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
    }
}