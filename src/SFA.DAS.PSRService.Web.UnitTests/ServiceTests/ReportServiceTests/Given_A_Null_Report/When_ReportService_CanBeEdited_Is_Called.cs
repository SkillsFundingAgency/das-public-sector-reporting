using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.ReportServiceTests.
    Given_A_Null_Report
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class When_ReportService_CanBeEdited_Is_Called
        : Given_A_Null_Report
    {
        private bool response;

        public When_ReportService_CanBeEdited_Is_Called()
        {
            response
                =
                SUT
                    .CanBeEdited(
                        StubReport);
        }

        [Test]
        public void Then_False_Is_Returned()
        {
            response
                .Should()
                .BeFalse();
        }

        [Test]
        public void Then_PeriodService_ReportIsForCurrentPeriod_Is_Not_Called()
        {
            MockPeriodService
                .Verify( 
                    m => m.ReportIsForCurrentPeriod(It.IsAny<Report>()), Times.Never);
        }
    }
}