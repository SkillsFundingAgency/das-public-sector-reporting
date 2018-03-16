using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SFA.DAS.PSRService.Web.Services;
using Assert = NUnit.Framework.Assert;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests
{
    [TestFixture]
    public class ReportServiceTest
    {
        private ReportService _reportService;

        [SetUp]
        public void SetUp()
        {
            _reportService = new ReportService(null, null);
        }

        [Test]
        public void TestCurrentPeriod()
        {
            var period1 = _reportService.GetCurrentReportPeriod(new DateTime(2017, 9, 30));
            var period2 = _reportService.GetCurrentReportPeriod(new DateTime(2017, 10, 1));

            Assert.AreEqual("1617", period1);
            Assert.AreEqual("1718", period2);
        }


        [Test]
        public void TestCurrentPeriodName()
        {
            var period1 = _reportService.GetCurrentReportPeriod(new DateTime(2017, 9, 30));
            var period2 = _reportService.GetCurrentReportPeriod(new DateTime(2017, 10, 1));

            var periodName1 = _reportService.GetCurrentReportPeriodName(period1);
            var periodName2 = _reportService.GetCurrentReportPeriodName(period2);

            Assert.AreEqual("1 April 2016 to 31 March 2017", periodName1);
            Assert.AreEqual("1 April 2017 to 31 March 2018", periodName2);
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
    }
}
