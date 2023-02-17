using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Domain.UnitTests.A_Period
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class A_Period
    {
        [TestCase("1617", "1617", true)]
        [TestCase("1617", "1516", false)]
        [TestCase("2021", "2122", false)]
        [TestCase("1718", "1718", true)]
        [TestCase("1718", "1819", false)]
        public void Is_Equatable_With_Another_Period(
            string left,
            string right,
            bool expectedEqual)
        {
            var leftPeriod =
                Period
                    .ParsePeriodString(left);

            var rightPeriod =
                Period
                    .ParsePeriodString(right);

            leftPeriod
                .Equals(rightPeriod)
                .Should()
                .Be(expectedEqual);
        }

        //[TestCase(2017, 9, 30, "1617")]
        //[TestCase(2017, 10, 1, "1617")]
        //[TestCase(2017, 4, 1, "1617")]
        //[TestCase(2017, 3, 31, "1516")]
        //public void Returns_Correct_PeriodString_For_Given_DateTime(
        //    int year,
        //    int month,
        //    int day,
        //    string expectedPeriodString)
        //{
        //    Period
        //        .FromInstantInPeriod(
        //            new DateTime(
        //                year,
        //                month,
        //                day))
        //        .PeriodString
        //        .Should()
        //        .Be(expectedPeriodString);
        //}

        //[TestCase(2017, 9, 30, "1 April 2016 to 31 March 2017")]
        //[TestCase(2017, 10, 1, "1 April 2016 to 31 March 2017")]
        //[TestCase(2017, 4, 1, "1 April 2016 to 31 March 2017")]
        //[TestCase(2017, 3, 31, "1 April 2015 to 31 March 2016")]
        //public void Returns_Correct_FullString_For_Given_DateTime(
        //int year,
        //int month,
        //int day,
        //string expectedFullString)
        //{
        //    Period
        //        .FromInstantInPeriod(
        //            new DateTime(
        //                year,
        //                month,
        //                day))
        //        .FullString
        //        .Should()
        //        .Be(expectedFullString);
        //}

        //[TestCase(2017, 9, 30, "2016")]
        //[TestCase(2017, 10, 1, "2016")]
        //[TestCase(2018, 4, 1, "2017")]
        //[TestCase(2017, 3, 31, "2015")]
        //public void Returns_Correct_StartYear_For_Given_DateTime(
        //    int year,
        //    int month,
        //    int day,
        //    string expectedStartYear)
        //{
        //    Period
        //        .FromInstantInPeriod(
        //            new DateTime(
        //                year,
        //                month,
        //                day))
        //        .StartYear
        //        .AsFourDigitString
        //        .Should()
        //        .Be(expectedStartYear);
        //}

        //[TestCase(2017, 9, 30, "2017")]
        //[TestCase(2017, 10, 1, "2017")]
        //[TestCase(2018, 4, 1, "2018")]
        //[TestCase(2017, 3, 31, "2016")]
        //public void Returns_Correct_EndYear_For_Given_DateTime(
        //    int year,
        //    int month,
        //    int day,
        //    string expectedEndYear)
        //{
        //    Period
        //        .FromInstantInPeriod(
        //            new DateTime(
        //                year,
        //                month,
        //                day))
        //        .EndYear
        //        .AsFourDigitString
        //        .Should()
        //        .Be(expectedEndYear);
        //}

        [TestCase("1617")]
        [TestCase("1718")]
        [TestCase("1819")]
        [TestCase("1516")]
        public void Has_PeriodString_Same_As_Passed_To_ParsePeriodString(
            string periodString)
        {
            Period
                .ParsePeriodString(periodString)
                .PeriodString
                .Should()
                .Be(periodString);
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
                        .TypeOf<ArgumentException>());
        }
    }
}