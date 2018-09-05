using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.PeriodServiceTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public sealed class A_PeriodService
    {
        [TestCase(2017, 9, 30, "1617")]
        [TestCase(2017, 10, 1,"1617")]
        [TestCase(2017, 4, 1, "1617")]
        [TestCase(2017, 3, 31, "1516")]
        [TestCase(2019, 3, 31, "1718")]
        [TestCase(2019, 4, 1, "1819")]
        public void Can_Provide_A_Current_Reporting_Period(
            int currentYear,
            int currentMonth,
            int currentDay,
            string expectedPeriodShortForm)
        {
            var mockDateTimeService = new Mock<IDateTimeService>();

            mockDateTimeService
                .Setup(
                    m => m.UtcNow)
                .Returns(
                    DateTime
                        .SpecifyKind(
                            new DateTime(currentYear, currentMonth, currentDay),
                            DateTimeKind.Utc));

            var SUT = new PeriodService(mockDateTimeService.Object);

            SUT
                .GetCurrentPeriod()
                .PeriodString
                .Should()
                .Be(expectedPeriodShortForm);
        }

        [TestCase(2017, 9, 30, "1617", true)]
        [TestCase(2017, 10, 1, "1516", false)]
        [TestCase(2017, 4, 1, "1617", true)]
        [TestCase(2017, 3, 31, "1617", false)]
        [TestCase(2019, 3, 31, "1617", false)]
        [TestCase(2019, 4, 1, "1819", true)]
        public void Can_Test_If_A_Given_ReportingPeriod_Is_Current(
            int currentYear,
            int currentMonth,
            int currentDay,
            string comparisonPeriodShortFormat,
            bool expectedComparisonResult)
        {
            var comparisonPeriod = Period.ParsePeriodString(comparisonPeriodShortFormat);

            var mockDateTimeService = new Mock<IDateTimeService>();

            mockDateTimeService
                .Setup(
                    m => m.UtcNow)
                .Returns(
                    DateTime
                        .SpecifyKind(
                            new DateTime(currentYear, currentMonth, currentDay),
                            DateTimeKind.Utc));

            var SUT = new PeriodService(mockDateTimeService.Object);

            SUT
                .PeiordIsCurrent(comparisonPeriod)
                .Should()
                .Be(expectedComparisonResult);
        }
    }
}