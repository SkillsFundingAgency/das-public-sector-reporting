using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Middleware.Authorization
{
    public abstract class AccountsClaimsAuthorizationHandler<TypeOfRequirement>
        : AuthorizationHandler<TypeOfRequirement>
        where TypeOfRequirement : IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            TypeOfRequirement requirement)
        {
            if (ContextResourceIsNotATypeWithRoutData(context))
                return Task.CompletedTask;

            if (RouteDataDoesNotContainAccountId(context))
                return Task.CompletedTask;

            if (UserDoesNotHaveAccountsClaim(context))
                return Task.CompletedTask;

            return
                AuthorizeRequirementAgainstCurrentAccountIdEmployerIdentifierInformation(
                    context,
                    requirement,
                    GetAccountsClaimsForUrlAccountId(
                        context.User,
                        GetAccountIdFromUrl(context)));
        }

        protected abstract Task AuthorizeRequirementAgainstCurrentAccountIdEmployerIdentifierInformation(AuthorizationHandlerContext context, TypeOfRequirement requirement, EmployerIdentifier employerIdentifier);

        private static string GetAccountIdFromUrl(AuthorizationHandlerContext context)
        {
            return ((ActionContext)context.Resource)
                .RouteData
                .Values
                    [RouteValues.HashedEmployerAccountId]
                .ToString()
                .ToUpper();
        }

        private EmployerIdentifier GetAccountsClaimsForUrlAccountId(ClaimsPrincipal user, string accountIdFromUrl)
        {
            var employerAccountClaim =
                user.FindFirst(c => c.Type.Equals(EmployerPsrsClaims.AccountsClaimsTypeIdentifier));
            var employerAccounts =
                JsonConvert.DeserializeObject<Dictionary<string, EmployerIdentifier>>(
                    employerAccountClaim
                    ?.Value);

            if (employerAccounts.ContainsKey(accountIdFromUrl))
            {
                return employerAccounts[accountIdFromUrl];
            }
            else
            {
                return new EmployerIdentifier
                {
                    AccountId = String.Empty,
                    EmployerName = String.Empty,
                    Role = String.Empty
                };
            }
        }

        private bool UserDoesNotHaveAccountsClaim(AuthorizationHandlerContext context)
        {
            return
                context
                    .User
                    .HasClaim(
                        claim
                            =>
                                claim
                                    .Type
                                    .Equals(
                                        EmployerPsrsClaims.AccountsClaimsTypeIdentifier
                                        , StringComparison.OrdinalIgnoreCase))
                == false;
        }

        private static bool RouteDataDoesNotContainAccountId(AuthorizationHandlerContext context)
        {
            return ((ActionContext)context.Resource)
                            .RouteData
                            .Values
                            .ContainsKey(
                                RouteValues.HashedEmployerAccountId) == false;
        }

        private static bool ContextResourceIsNotATypeWithRoutData(AuthorizationHandlerContext context)
        {
            return (context.Resource is ActionContext) == false;
        }
    }
}