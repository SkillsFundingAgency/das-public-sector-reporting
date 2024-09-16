using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PSRService.Web.Middleware.Authorization;

namespace SFA.DAS.PSRService.Web.UnitTests.Middleware.RoleRequierments.Given_RouteData_Is_Null;

[ExcludeFromCodeCoverage]
public abstract class Given_RouteData_Is_Null
    : GivenWhenThen<AccountsClaimsAuthorizationHandler<TestRequirement>>
{
    protected AuthorizationHandlerContext HandlerContext;

    protected override void Given()
    {
        SUT = new StubUserHasRoleForAccount();

        var validResourceType = new ActionContext();

        validResourceType.RouteData = null;

        HandlerContext =
            new AuthorizationHandlerContext(
                requirements: new List<IAuthorizationRequirement> {new TestRequirement()}
                , user: new ClaimsPrincipal()
                , resource: validResourceType
            );
    }
}