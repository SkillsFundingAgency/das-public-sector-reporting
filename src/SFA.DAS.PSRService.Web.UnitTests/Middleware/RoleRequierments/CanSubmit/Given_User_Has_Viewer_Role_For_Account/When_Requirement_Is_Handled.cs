using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;

namespace SFA.DAS.PSRService.Web.UnitTests.Middleware.RoleRequierments.CanSubmit.Given_User_Has_Viewer_Role_For_Account;

[ExcludeFromCodeCoverage]
public sealed class When_Requirement_Is_Handled : Given_User_Has_Viewer_Role_For_Account
{
    protected override void When()
    {
        SUT.HandleAsync(HandlerContext);
    }

    [Test]
    public void Then_Requirement_Is_Not_Satisfied()
    {
        HandlerContext.HasSucceeded.Should().BeFalse();
    }
}