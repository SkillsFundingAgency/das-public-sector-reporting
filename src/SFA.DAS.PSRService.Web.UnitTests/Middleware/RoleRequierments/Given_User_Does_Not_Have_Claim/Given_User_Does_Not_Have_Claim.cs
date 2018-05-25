using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Middleware.Authorization;
using SFA.DAS.PSRService.Web.UnitTests.Middleware.HasRole;

namespace SFA.DAS.PSRService.Web.UnitTests.Middleware.RoleRequierments.Given_User_Does_Not_Have_Claim
{
    [ExcludeFromCodeCoverage]
    public abstract class Given_User_Does_Not_Have_Claim
        : GivenWhenThen<AccountsClaimsAuthorizationHandler<TestRequirement>>
    {
        protected AuthorizationHandlerContext HandlerContext;

        protected override void Given()
        {
            SUT = new StubUserHasRoleForAccount();

            var validResourceType = new ActionContext();

            validResourceType.RouteData = new RouteData();

            validResourceType
                .RouteData
                .Values[RouteValues.EmployerAccountId] = "ACCOUNTID";

            HandlerContext =
                new AuthorizationHandlerContext(
                    requirements: new List<IAuthorizationRequirement> {new TestRequirement()}
                    , user: new ClaimsPrincipal()
                    , resource: validResourceType
                );
        }
    }
}