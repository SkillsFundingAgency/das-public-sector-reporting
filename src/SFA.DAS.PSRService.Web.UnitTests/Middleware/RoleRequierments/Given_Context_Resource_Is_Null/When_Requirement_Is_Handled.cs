namespace SFA.DAS.PSRService.Web.UnitTests.Middleware.RoleRequierments.Given_Context_Resource_Is_Null;

[ExcludeFromCodeCoverage]
public sealed class When_Requirement_Is_Handled :Given_Context_Resource_Is_Null
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