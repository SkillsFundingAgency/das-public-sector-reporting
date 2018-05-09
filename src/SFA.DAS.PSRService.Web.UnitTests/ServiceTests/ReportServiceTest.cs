using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Services;
using Assert = NUnit.Framework.Assert;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests
{
    [TestFixture]
    public class ReportServiceTest
    {
        private ReportService _reportService;
        private Mock<IWebConfiguration> _webConfigurationMock;
        private Mock<IPeriodService> _periodServiceMock;

        [SetUp]
        public void SetUp()
        {
            _webConfigurationMock = new Mock<IWebConfiguration>(MockBehavior.Strict);
            _periodServiceMock = new Mock<IPeriodService>(MockBehavior.Strict);

            _reportService = new ReportService(_webConfigurationMock.Object, null, _periodServiceMock.Object);
        }

   

        [Test]
        public void TestSubmissionClosedWhenDateInPastIsFalse()
        {
            _webConfigurationMock.Setup(s => s.SubmissionClose).Returns(DateTime.UtcNow.AddDays(-3));

            var result = _reportService.IsSubmissionsOpen();

            Assert.IsFalse(result);
        }

        [Test]
        public void TestSubmissionClosedWhenDateInFutureIsTrue()
        {
            _webConfigurationMock.Setup(s => s.SubmissionClose).Returns(DateTime.UtcNow.AddDays(+3));

            var result = _reportService.IsSubmissionsOpen();

            Assert.IsTrue(result);
        }

        [Test]
        public void TestSubmissionClosedWhenDateIsTodayIsFalse()
        {
            _webConfigurationMock.Setup(s => s.SubmissionClose).Returns(DateTime.UtcNow);

            var result = _reportService.IsSubmissionsOpen();

            Assert.IsFalse(result);
        }
    }
}
