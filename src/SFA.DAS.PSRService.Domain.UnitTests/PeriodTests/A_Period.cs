using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Domain.UnitTests.A_Period
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class A_Period
    {
        [TestCase(2017, 9, 30, "1617")]
        [TestCase(2017, 10, 1, "1617")]
        [TestCase(2017, 4, 1, "1617")]
        [TestCase(2017, 3, 31, "1516")]
        public void Returns_Correct_PeriodString_For_Given_DateTime(
            int year,
            int month,
            int day,
            string expectedPeriodString)
        {
            Assert
                .AreEqual(
                    expectedPeriodString, 
                    new Period(new DateTime(year, month, day)).PeriodString);
        }

        [TestCase(2017, 9, 30, "1 April 2016 to 31 March 2017")]
        [TestCase(2017, 10, 1, "1 April 2016 to 31 March 2017")]
        [TestCase(2017, 4, 1, "1 April 2016 to 31 March 2017")]
        [TestCase(2017, 3, 31, "1 April 2015 to 31 March 2016")]
        public void Returns_Correct_FullString_For_Given_DateTime(
        int year,
        int month,
        int day,
        string expectedFullString)
        {
            Assert.AreEqual(expectedFullString, new Period(new DateTime(year, month, day)).FullString);
        }

        [TestCase(2017, 9, 30, "2016")]
        [TestCase(2017, 10, 1, "2016")]
        [TestCase(2018, 4, 1, "2017")]
        [TestCase(2017, 3, 31, "2015")]
        public void Returns_Correct_StartYear_For_Given_DateTime(
            int year,
            int month,
            int day,
            string expectedStartYear)
        {
            Assert.AreEqual(expectedStartYear, new Period(new DateTime(year, month, day)).StartYear);
        }

        [TestCase(2017, 9, 30, "2017")]
        [TestCase(2017, 10, 1, "2017")]
        [TestCase(2018, 4, 1, "2018")]
        [TestCase(2017, 3, 31, "2016")]
        public void Returns_Correct_EndYear_For_Given_DateTime(
            int year,
            int month,
            int day,
            string expectedEndYear)
        {
            Assert.AreEqual(expectedEndYear, new Period(new DateTime(year, month, day)).EndYear);
        }

        [Test]
        public void Throws_ArgumentException_When_Null_Passed_To_ParsePeriodString()
        {
            Assert
                .That(
                    () => Period.ParsePeriodString(null),
                    Throws
                        .Exception
                        .TypeOf<ArgumentException>()
                        .With.Property(nameof(ArgumentException.ParamName))
                        .EqualTo("periodString"));
        }

        [TestCase("morethanfourcharacters")]
        [TestCase("four")]
        [TestCase("")]
        [TestCase("     ")]
        [TestCase("1")]
        [TestCase("12")]
        [TestCase("123")]
        public void Throws_ArgumentException_When_Invalid_String_Passed_To_ParsePeriodString(
            string parseArgument)
        {
            Assert
                .That(
                    () => Period.ParsePeriodString(parseArgument),
                    Throws
                        .Exception
                        .TypeOf<ArgumentException>()
                        .With.Property(nameof(ArgumentException.ParamName))
                        .EqualTo("periodString"));
        }
    }
}