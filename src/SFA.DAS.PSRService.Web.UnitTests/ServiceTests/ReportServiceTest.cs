using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
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

        [SetUp]
        public void SetUp()
        {
            _webConfigurationMock = new Mock<IWebConfiguration>(MockBehavior.Strict);
            _reportService = new ReportService(_webConfigurationMock.Object, null);
        }

        [Test]
        public void TestCurrentPeriod()
        {
            var period1 = _reportService.GetReportPeriod(new DateTime(2017, 9, 30));
            var period2 = _reportService.GetReportPeriod(new DateTime(2017, 10, 1));
            var period3 = _reportService.GetReportPeriod(new DateTime(2017, 4, 1));
            var period4 = _reportService.GetReportPeriod(new DateTime(2017, 3, 31));

            Assert.AreEqual("1617", period1);
            Assert.AreEqual("1617", period2);
            Assert.AreEqual("1617", period3);
            Assert.AreEqual("1516", period4);
        }


        [Test]
        public void TestCurrentPeriodName()
        {
            var period1 = _reportService.GetReportPeriod(new DateTime(2017, 9, 30));
            var period2 = _reportService.GetReportPeriod(new DateTime(2017, 10, 1));
            var period3 = _reportService.GetReportPeriod(new DateTime(2017, 4, 1));
            var period4 = _reportService.GetReportPeriod(new DateTime(2017, 3, 31));

            var periodName1 = _reportService.GetCurrentReportPeriodName(period1);
            var periodName2 = _reportService.GetCurrentReportPeriodName(period2);
            var periodName3 = _reportService.GetCurrentReportPeriodName(period3);
            var periodName4 = _reportService.GetCurrentReportPeriodName(period4);

            Assert.AreEqual("1 April 2016 to 31 March 2017", periodName1);
            Assert.AreEqual("1 April 2016 to 31 March 2017", periodName2);
            Assert.AreEqual("1 April 2016 to 31 March 2017", periodName3);
            Assert.AreEqual("1 April 2015 to 31 March 2016", periodName4);
        }

        [Test]
        public void TestCurrentPeriodNameFailsWhenNullPassed()
        {
            try
            {
                var periodName1 = _reportService.GetCurrentReportPeriodName(null);
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("period", ex.ParamName);
                return;
            }

            Assert.Fail("Correct exception wasn't thrown");
        }

        [Test]
        public void TestCurrentPeriodNameFailsWhenInvalidStringPassed()
        {
            try
            {
                var periodName1 = _reportService.GetCurrentReportPeriodName("null!");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("period", ex.ParamName);
                return;
            }

            Assert.Fail("Correct exception wasn't thrown");
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
