namespace SFA.DAS.PSRService.Web.UnitTests.Middleware.RoleRequierments.Given_RouteData_Values_Do_Not_Contain_AccountId;

[ExcludeFromCodeCoverage]
public sealed class When_Requirement_Is_Handled : Given_RouteData_Values_Do_Not_Contain_AccountId
{
    protected override async Task When()
    {
        await Sut.HandleAsync(HandlerContext);
    }

    [Test]
    public void Then_Requirement_Is_Not_Satisfied()
    {
        HandlerContext.HasSucceeded.Should().BeFalse();
    }
}