using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Domain.UnitTest
{
    [TestFixture]
    public class PeriodTests
    {
        [Test]
        public void And_A_Valid_Date_Is_Used_Then_Return_Period()
        {
            var period = new Period(DateTime.UtcNow);

            Assert.AreEqual(true, period.IsCurrent);
            Assert.AreEqual("1718", period.PeriodString);
            Assert.AreEqual("2017", period.StartYear);
            Assert.AreEqual("2018", period.EndYear);
            Assert.AreEqual("1 April 2017 to 31 March 2018", period.FullString);
        }

        [Test]
        public void TestCurrentPeriod()
        {
             var period1 = new Period(new DateTime(2017, 9, 30));
            var period2 = new Period(new DateTime(2017, 10, 1));
            var period3 = new Period(new DateTime(2017, 4, 1));
            var period4 = new Period(new DateTime(2017, 3, 31));

            Assert.AreEqual("1617", period1.PeriodString);
            Assert.AreEqual("1617", period2.PeriodString);
            Assert.AreEqual("1617", period3.PeriodString);
            Assert.AreEqual("1516", period4.PeriodString);
        }


        [Test]
        public void TestCurrentPeriodName()
        {
            var period1 = new Period(new DateTime(2017, 9, 30));
            var period2 = new Period(new DateTime(2017, 10, 1));
            var period3 = new Period(new DateTime(2017, 4, 1));
            var period4 = new Period(new DateTime(2017, 3, 31));



            Assert.AreEqual("1 April 2016 to 31 March 2017", period1.FullString);
            Assert.AreEqual("1 April 2016 to 31 March 2017", period2.FullString);
            Assert.AreEqual("1 April 2016 to 31 March 2017", period3.FullString);
            Assert.AreEqual("1 April 2015 to 31 March 2016", period4.FullString);
        }

        [Test]
        public void TestCurrentPeriodNameFailsWhenNullPassed()
        {
            try
            {
                var periodName1 = new Period(null).FullString;
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
            var expectedException
                =
                Assert
                    .Catch<ArgumentException>(
                        () => new Period("null!"));

            Assert
                .AreEqual(
                    "period"
                    , expectedException
                        .ParamName);
        }
    }
}