namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.ReportServiceTests.Can_Be_Edited.Given_A_Not_Submitted_Report.And_Report_Is_Not_For_Current_Period;

[ExcludeFromCodeCoverage]
[TestFixture]
public class When_ReportService_CanBeEdited_Is_Called : And_Report_Is_Not_For_Current_Period
{
    private readonly bool _response;

    public When_ReportService_CanBeEdited_Is_Called()
    {
        _response = Sut.CanBeEdited(StubReport);
    }

    [Test]
    public void Then_False_Is_Returned()
    {
        _response.Should().BeFalse();
    }
}