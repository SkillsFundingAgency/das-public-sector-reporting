using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.ReportServiceTests.Can_Be_Edited.Given_A_Not_Submitted_Report.
    And_Report_Is_For_Current_Period
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class When_ReportService_CanBeEdited_Is_Called
        : And_Report_Is_For_Current_Period
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
        public void Then_True_Is_Returned()
        {
            response
                .Should()
                .BeTrue();
        }
    }
}