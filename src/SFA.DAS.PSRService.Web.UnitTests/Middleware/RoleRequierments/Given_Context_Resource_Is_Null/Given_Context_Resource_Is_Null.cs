using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.PSRService.Web.Middleware.Authorization;
using SFA.DAS.PSRService.Web.UnitTests.Middleware.HasRole;

namespace SFA.DAS.PSRService.Web.UnitTests.Middleware.RoleRequierments.Given_Context_Resource_Is_Null
{
    [ExcludeFromCodeCoverage]
    public abstract class Given_Context_Resource_Is_Null
        : GivenWhenThen<AccountsClaimsAuthorizationHandler<TestRequirement>>
    {
        protected AuthorizationHandlerContext HandlerContext;

        protected override void Given()
        {
            SUT = new StubUserHasRoleForAccount();

            HandlerContext =
                new AuthorizationHandlerContext(
                    requirements: new List<IAuthorizationRequirement> {new TestRequirement()}
                    , user: new ClaimsPrincipal()
                    , resource: null
                );
        }
    }
}