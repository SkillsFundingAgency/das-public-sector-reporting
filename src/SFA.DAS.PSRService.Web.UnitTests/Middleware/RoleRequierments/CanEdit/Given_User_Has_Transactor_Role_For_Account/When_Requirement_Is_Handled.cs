namespace SFA.DAS.PSRService.Web.UnitTests.Middleware.RoleRequierments.CanEdit.Given_User_Has_Transactor_Role_For_Account;

[ExcludeFromCodeCoverage]
public sealed class When_Requirement_Is_Handled :Given_User_Has_Owner_Role_For_Account
{
    protected override async Task When()
    {
        await Sut.HandleAsync(HandlerContext);
    }

    [Test]
    public void Then_Requirement_Is_Satisfied()
    {
        HandlerContext.HasSucceeded.Should().BeTrue();
    }
}