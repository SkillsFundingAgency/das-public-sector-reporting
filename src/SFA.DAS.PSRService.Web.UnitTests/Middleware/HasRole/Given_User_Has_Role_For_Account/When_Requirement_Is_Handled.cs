using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace SFA.DAS.PSRService.Web.UnitTests.Middleware.HasRole.Given_User_Has_Role_For_Account
{
    [ExcludeFromCodeCoverage]
    public sealed class When_Requirement_Is_Handled
    :Given_User_Has_Role_For_Account
    {
        protected override void When()
        {
            SUT
                .HandleAsync(
                    HandlerContext);
        }

        [Test]
        public void Then_Requirement_Is_Satisfied()
        {
            Assert
                .True(
                    HandlerContext
                        .HasSucceeded);
        }
    }
}