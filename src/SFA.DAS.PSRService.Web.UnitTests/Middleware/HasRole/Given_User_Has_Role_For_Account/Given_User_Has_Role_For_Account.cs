using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.PSRService.Web.Middleware.Authorization;

namespace SFA.DAS.PSRService.Web.UnitTests.Middleware.HasRole.Given_User_Has_Role_For_Account
{
    [ExcludeFromCodeCoverage]
    public abstract class Given_User_Has_Role_For_Account
        : GivenWhenThen<UserHasRoleForAccount<TestRequirement>>
    {
        protected AuthorizationHandlerContext HandlerContext;

        protected override void Given()
        {
            SUT = new StubUserHasRoleForAccount();

            HandlerContext =
                new AuthorizationHandlerContext(
                    requirements: new List<IAuthorizationRequirement> {new TestRequirement()}
                    , user: new ClaimsPrincipal()
                    , resource: new object()
                );
        }
    }
}