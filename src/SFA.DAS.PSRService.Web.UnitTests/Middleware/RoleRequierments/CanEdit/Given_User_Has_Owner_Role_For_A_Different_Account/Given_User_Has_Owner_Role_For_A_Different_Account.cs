﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
using SFA.DAS.PSRService.Web.Middleware.Authorization;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.UnitTests.Middleware.RoleRequierments.CanEdit.Given_User_Has_Owner_Role_For_A_Different_Account;

[ExcludeFromCodeCoverage]
public abstract class Given_User_Has_Owner_Role_For_A_Different_Account : GivenWhenThen<CanEditReportHandler>
{
    protected AuthorizationHandlerContext HandlerContext;

    private static string AccountId => "TESTACCOUNTID";

    protected override void Given()
    {
        Sut = new CanEditReportHandler();

        HandlerContext = new AuthorizationHandlerContext(
            requirements: new List<IAuthorizationRequirement> { new CanEditReport() },
            user: BuildUserWithRequiredRoleForAccount(),
            resource: BuildResourceWithAccountID());
    }

    private static object BuildResourceWithAccountID()
    {
        var resourceContext = new ActionContext();

        var routeData = new RouteData
        {
            Values =
            {
                [RouteValues.HashedEmployerAccountId] = AccountId
            }
        };

        resourceContext.RouteData = routeData;

        return resourceContext;
    }

    private static ClaimsPrincipal BuildUserWithRequiredRoleForAccount()
    {
        return new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(
                EmployerPsrsClaims
                    .AccountsClaimsTypeIdentifier
                , EmployerIdentifierWithOwnerRoleForAccount()
                , JsonClaimValueTypes.Json)
        ]));
    }

    private static string EmployerIdentifierWithOwnerRoleForAccount()
    {
        return JsonConvert.SerializeObject(new Dictionary<string, EmployerIdentifier>
        {
            ["SOMEOTHERACCOUNT"] = new()
            {
                AccountId = AccountId,
                Role = EmployerPsrsRoleNames.Owner
            }
        });
    }
}