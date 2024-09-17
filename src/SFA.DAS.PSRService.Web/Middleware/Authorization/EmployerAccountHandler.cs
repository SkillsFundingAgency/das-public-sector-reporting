using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Middleware.Authorization;

public class EmployerAccountHandler(IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<EmployerAccountRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployerAccountRequirement requirement)
    {
        if (!httpContextAccessor.HttpContext.Request.RouteValues.ContainsKey(RouteValues.HashedEmployerAccountId))
        {
            return Task.CompletedTask;
        }

        var accountIdFromUrl = httpContextAccessor.HttpContext.Request.RouteValues[RouteValues.HashedEmployerAccountId].ToString().ToUpper();

        if (!context.User.HasClaim(c => c.Type.Equals(EmployerPsrsClaims.AccountsClaimsTypeIdentifier)))
        {
            return Task.CompletedTask;
        }

        var employerAccountClaim = context.User.FindFirst(c => c.Type.Equals(EmployerPsrsClaims.AccountsClaimsTypeIdentifier));
        var employerAccounts = JsonConvert.DeserializeObject<Dictionary<string, EmployerIdentifier>>(employerAccountClaim?.Value);

        if (employerAccountClaim == null || !employerAccounts.ContainsKey(accountIdFromUrl))
        {
            return Task.CompletedTask;
        }
            
        if (context.Resource is AuthorizationFilterContext mvcContext && !mvcContext.HttpContext.Items.ContainsKey(ContextItemKeys.EmployerIdentifier))
        {
            mvcContext.HttpContext.Items.Add(ContextItemKeys.EmployerIdentifier, employerAccounts.GetValueOrDefault(accountIdFromUrl));
        }

        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}