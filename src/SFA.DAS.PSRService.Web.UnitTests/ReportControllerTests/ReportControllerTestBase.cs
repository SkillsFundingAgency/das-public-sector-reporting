using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;
using SFA.DAS.PSRService.Web.ViewModels;

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
        private Mock<IPeriodService> _periodServiceMock;
        private Mock<ILogger<ReportController>> _mockLogging;
        protected IList<Report> _reportList = ReportTestModelBuilder.ReportsWithValidSections();
        private EmployerIdentifier _employerIdentifier;

        [SetUp]
        public void SetUp()
        {
            _mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            _mockReportService = new Mock<IReportService>(MockBehavior.Strict);
            _mockLogging = new Mock<ILogger<ReportController>>(MockBehavior.Strict);
            _employeeAccountServiceMock = new Mock<IEmployerAccountService>(MockBehavior.Strict);
            _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
            _periodServiceMock = new Mock<IPeriodService>(MockBehavior.Strict);


            _periodServiceMock.Setup(s => s.GetCurrentPeriod()).Returns(new Period(DateTime.UtcNow));

            _controller = new ReportController(_mockLogging.Object, _mockReportService.Object, _employeeAccountServiceMock.Object, _userServiceMock.Object, null, _periodServiceMock.Object) { Url = _mockUrlHelper.Object };
            _employerIdentifier = new EmployerIdentifier() { AccountId = "ABCDE", EmployerName = "EmployerName" };

            _employeeAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(It.IsAny<HttpContext>()))
                .Returns(_employerIdentifier);
            _employeeAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(null))
                .Returns(_employerIdentifier);

            _userServiceMock.Setup(s => s.GetUserModel(null)).Returns(new UserModel());

            
            

            //  _mockReportService.Setup(s => s.GetCurrentReportPeriod()).Returns("1617");
        }


    }
}
