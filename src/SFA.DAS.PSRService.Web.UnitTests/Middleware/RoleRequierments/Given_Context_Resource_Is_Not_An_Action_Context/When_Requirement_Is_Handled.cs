namespace SFA.DAS.PSRService.Web.UnitTests.Middleware.RoleRequierments.Given_Context_Resource_Is_Not_An_Action_Context;

[ExcludeFromCodeCoverage]
public sealed class When_Requirement_Is_Handled
    : Given_Context_Resource_Is_Not_An_Action_Context
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