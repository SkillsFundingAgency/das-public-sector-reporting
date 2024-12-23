﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Middleware.Authorization;

public abstract class AccountsClaimsAuthorizationHandler<TRequirement> : AuthorizationHandler<TRequirement> where TRequirement : IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement)
    {
        var routeData = (context.Resource as ActionContext)?.RouteData ?? (context.Resource as DefaultHttpContext)?.GetRouteData();
        
        if (routeData == null)
        {
            return Task.CompletedTask;
        }

        if (RouteDataDoesNotContainAccountId(routeData))
        {
            return Task.CompletedTask;
        }

        if (UserDoesNotHaveAccountsClaim(context))
        {
            return Task.CompletedTask;
        }

        return AuthorizeRequirementAgainstCurrentAccountIdEmployerIdentifierInformation(context, requirement, GetAccountsClaimsForUrlAccountId(context.User, GetAccountIdFromUrl(routeData)));
    }

    protected abstract Task AuthorizeRequirementAgainstCurrentAccountIdEmployerIdentifierInformation(AuthorizationHandlerContext context, TRequirement requirement, EmployerIdentifier employerIdentifier);

    private static string GetAccountIdFromUrl(RouteData routeData)
    {
        return routeData.Values[RouteValues.HashedEmployerAccountId].ToString().ToUpper();
    }

    private static EmployerIdentifier GetAccountsClaimsForUrlAccountId(ClaimsPrincipal user, string accountIdFromUrl)
    {
        var employerAccountClaim = user.FindFirst(c => c.Type.Equals(EmployerPsrsClaims.AccountsClaimsTypeIdentifier));
        var employerAccounts = JsonConvert.DeserializeObject<Dictionary<string, EmployerIdentifier>>(employerAccountClaim?.Value);

        if (employerAccounts.TryGetValue(accountIdFromUrl, out var id))
        {
            return id;
        }

        return new EmployerIdentifier
        {
            AccountId = string.Empty,
            EmployerName = string.Empty,
            Role = string.Empty
        };
    }

    private static bool UserDoesNotHaveAccountsClaim(AuthorizationHandlerContext context)
    {
        return !context.User.HasClaim(claim => claim.Type.Equals(EmployerPsrsClaims.AccountsClaimsTypeIdentifier, StringComparison.OrdinalIgnoreCase));
    }

    private static bool RouteDataDoesNotContainAccountId(RouteData routeData)
    {
        return !routeData.Values.ContainsKey(RouteValues.HashedEmployerAccountId);
    }
}