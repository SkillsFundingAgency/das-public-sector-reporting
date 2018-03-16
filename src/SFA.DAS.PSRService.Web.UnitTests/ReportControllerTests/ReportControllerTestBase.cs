using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests
{
    [TestFixture]
    public class ReportControllerTestBase
    {
        protected ReportController _controller;
        protected Mock<ILogger<ReportController>> _mockLogging;
        protected Mock<IReportService> _mockReportService;
        protected Mock<IUrlHelper> _mockUrlHelper;
        protected IList<Report> _reportList;

        [SetUp]
        public void SetUp()
        {
            _mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            _mockReportService = new Mock<IReportService>(MockBehavior.Strict);
            _mockLogging = new Mock<ILogger<ReportController>>(MockBehavior.Strict);
            _controller = new ReportController(_mockLogging.Object, _mockReportService.Object) { Url = _mockUrlHelper.Object };

            _reportList = new List<Report>()
            {
                new Report()
                {
                    Id = Guid.NewGuid(),
                    ReportingPeriod = "1718",
                    EmployerId = 12345,
                    OrganisationName = "Organisation 1"
                },
                new Report()
                {
                    Id = Guid.NewGuid(),
                    ReportingPeriod = "1617",
                    EmployerId = 12345,
                    OrganisationName = "Organisation 1"
                },
                new Report()
                {
                    Id = Guid.NewGuid(),
                    ReportingPeriod = "1718",
                    EmployerId = 56789,
                    OrganisationName = "Organisation 2"
                }
            };

           
        }

    }
}
