using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.ReportServiceTests
{
    [TestFixture]
    public class Given_I_Calculate_A_Report_Period
    {
        private ReportService _reportService;
        private Mock<IMediator> _mediatorMock;
        private Mock<IWebConfiguration> _webConfigurationMock;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _webConfigurationMock = new Mock<IWebConfiguration>(MockBehavior.Strict);
            _reportService = new ReportService(_webConfigurationMock.Object, _mediatorMock.Object);
        }

        [Test]
        public void And_A_Valid_Date_Is_Used_Then_Return_Period()
        {


           var period = _reportService.GetPeriod("1718");

           Assert.AreEqual("2017", period.StartYear);
            Assert.AreEqual("2018", period.EndYear);
            Assert.AreEqual("1 April 2017 to 31 March 2018",period.FullString);
        }

    }
}
