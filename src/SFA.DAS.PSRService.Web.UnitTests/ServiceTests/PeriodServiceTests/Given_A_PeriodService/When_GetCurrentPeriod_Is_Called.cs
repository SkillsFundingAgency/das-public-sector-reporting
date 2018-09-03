using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.PeriodServiceTests.Given_A_PeriodService
{
    [ExcludeFromCodeCoverage]
    [TestFixture(2017, 9, 30)]
    [TestFixture(2017, 10, 1)]
    [TestFixture(2017, 4, 1)]
    [TestFixture(2017, 3, 31)]
    public sealed class When_GetCurrentPeriod_Is_Called
    : Given_A_PeriodService
    {
        private Period returnedPeriod;

        public When_GetCurrentPeriod_Is_Called(
            int year,
            int month,
            int day):base(year, month, day)
        {
            returnedPeriod = SUT.GetCurrentPeriod();
        }

        [Test]
        public void Then_A_Period_For_Current_Instant_Is_Returned()
        {
            var expectedPeriod
                =
                Period
                    .FromInstantInPeriod(
                        CurrentInstant);

            expectedPeriod
                .Equals(
                    returnedPeriod)
                .Should()
                .BeTrue("returned period: {0}, should have been: {1}", returnedPeriod.PeriodString, expectedPeriod.PeriodString );
        }
    }
}