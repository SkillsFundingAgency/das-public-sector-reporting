using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.ReportServiceTests.Given_A_Not_Submitted_Report.And_Report_Is_Not_For_Current_Period
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class When_ReportService_CanBeEdited_Is_Called
    : And_Report_Is_Not_For_Current_Period
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
    }
}