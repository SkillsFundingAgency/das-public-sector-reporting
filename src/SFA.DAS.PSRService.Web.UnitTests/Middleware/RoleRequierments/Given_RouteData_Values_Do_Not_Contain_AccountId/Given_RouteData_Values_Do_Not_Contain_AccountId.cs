using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SFA.DAS.PSRService.Web.Middleware.Authorization;

namespace SFA.DAS.PSRService.Web.UnitTests.Middleware.RoleRequierments.Given_RouteData_Values_Do_Not_Contain_AccountId;

[ExcludeFromCodeCoverage]
public abstract class Given_RouteData_Values_Do_Not_Contain_AccountId : GivenWhenThen<AccountsClaimsAuthorizationHandler<TestRequirement>>
{
    protected AuthorizationHandlerContext HandlerContext;

    protected override void Given()
    {
        Sut = new StubUserHasRoleForAccount();

        var validResourceType = new ActionContext
        {
            RouteData = new RouteData()
        };

        HandlerContext = new AuthorizationHandlerContext(
            requirements: new List<IAuthorizationRequirement> { new TestRequirement() },
            user: new ClaimsPrincipal(),
            resource: validResourceType
        );
    }
}