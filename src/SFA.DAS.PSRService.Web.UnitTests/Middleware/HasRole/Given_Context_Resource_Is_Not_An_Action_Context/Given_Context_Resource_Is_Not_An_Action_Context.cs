using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.PSRService.Web.Middleware.Authorization;

namespace SFA.DAS.PSRService.Web.UnitTests.Middleware.HasRole.Given_Context_Resource_Is_Not_An_Action_Context
{
    [ExcludeFromCodeCoverage]
    public abstract class Given_Context_Resource_Is_Not_An_Action_Context
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