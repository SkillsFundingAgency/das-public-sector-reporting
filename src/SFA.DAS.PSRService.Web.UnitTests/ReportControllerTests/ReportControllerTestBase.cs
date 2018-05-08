using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests
{
    [TestFixture]
    public class ReportControllerTestBase
    {
        protected ReportController _controller;
        protected Mock<IReportService> _mockReportService;
        protected Mock<IUrlHelper> _mockUrlHelper;
        private Mock<IEmployerAccountService> _employeeAccountServiceMock;
        public Mock<IUserService> _userServiceMock;
        protected IList<Report> _reportList;
        private EmployerIdentifier _employerIdentifier;

        [SetUp]
        public void SetUp()
        {
            _mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            _mockReportService = new Mock<IReportService>(MockBehavior.Strict);
            new Mock<ILogger<ReportController>>(MockBehavior.Strict);
            _employeeAccountServiceMock = new Mock<IEmployerAccountService>(MockBehavior.Strict);
            _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
            _controller = new ReportController(_mockReportService.Object,_employeeAccountServiceMock.Object, _userServiceMock.Object, null) { Url = _mockUrlHelper.Object };
            _employerIdentifier = new EmployerIdentifier() { AccountId = "ABCDE", EmployerName = "EmployerName" };

            _employeeAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(It.IsAny<HttpContext>()))
                .Returns(_employerIdentifier);
            _employeeAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(null))
                .Returns(_employerIdentifier);

            _userServiceMock.Setup(s => s.GetUserModel(null)).Returns(new UserModel());

            _reportList = new List<Report>()
            {
                new Report()
                {
                    Id = Guid.NewGuid(),
                    ReportingPeriod = "1718",
                    EmployerId = "ABCDE",
                    OrganisationName = "Organisation 1"
                },
                new Report()
                {
                    Id = Guid.NewGuid(),
                    ReportingPeriod = "1617",
                    EmployerId = "ABCDE",
                    OrganisationName = "Organisation 1"
                },
                new Report()
                {
                    Id = Guid.NewGuid(),
                    ReportingPeriod = "1718",
                    EmployerId = "VWXYZ",
                    OrganisationName = "Organisation 2"
                }
            };

            _mockReportService.Setup(s => s.GetCurrentReportPeriod()).Returns("1617");
        }

    }
}
