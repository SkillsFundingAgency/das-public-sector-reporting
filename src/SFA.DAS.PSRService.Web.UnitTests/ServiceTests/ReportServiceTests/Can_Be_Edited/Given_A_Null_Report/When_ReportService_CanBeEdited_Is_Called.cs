using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.ReportServiceTests.Can_Be_Edited.
    Given_A_Null_Report;

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
            Sut
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