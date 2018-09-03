using System;
using System.Diagnostics.CodeAnalysis;
using Moq;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.PeriodServiceTests.Given_A_PeriodService
{
    [ExcludeFromCodeCoverage]
    public abstract class Given_A_PeriodService : GivenWhenThen<IPeriodService>
    {
        protected DateTime CurrentInstant;

        protected Given_A_PeriodService(int year, int month, int day)
        {
            CurrentInstant
                =
                DateTime
                    .SpecifyKind(
                        new DateTime(year, month, day),
                        DateTimeKind.Utc);

            var mockDateTimeService = new Mock<IDateTimeService>();

            mockDateTimeService
                .Setup(
                    m => m.UtcNow)
                .Returns(
                    CurrentInstant);

            SUT = new PeriodService(mockDateTimeService.Object);
        }
    }
}