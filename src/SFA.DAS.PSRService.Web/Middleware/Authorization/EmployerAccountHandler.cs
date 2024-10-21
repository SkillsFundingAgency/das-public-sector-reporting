using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Middleware.Authorization;

public class EmployerAccountHandler(IHttpContextAccessor httpContextAccessor, ILogger<EmployerAccountHandler> logger) : AuthorizationHandler<EmployerAccountRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployerAccountRequirement requirement)
    {
        if (!httpContextAccessor.HttpContext.Request.RouteValues.TryGetValue(RouteValues.HashedEmployerAccountId, out var hashedAccountId))
        {
            logger.LogInformation("EmployerAccountHandler authorization failed because no HashedEmployerAccountId was provided.");
            return Task.CompletedTask;
        }

        var accountIdFromUrl = hashedAccountId.ToString().ToUpper();

        if (!context.User.HasClaim(c => c.Type.Equals(EmployerPsrsClaims.AccountsClaimsTypeIdentifier)))
        {
            logger.LogInformation("EmployerAccountHandler authorization failed because http://das/employer/identity/claims/associatedAccounts was empty");
            return Task.CompletedTask;
        }

        var employerAccountClaim = context.User.FindFirst(c => c.Type.Equals(EmployerPsrsClaims.AccountsClaimsTypeIdentifier));
        var employerAccounts = JsonConvert.DeserializeObject<Dictionary<string, EmployerIdentifier>>(employerAccountClaim?.Value);

        if (employerAccountClaim == null || !employerAccounts.ContainsKey(accountIdFromUrl))
        {
            if (employerAccountClaim == null)
            {
                logger.LogInformation("EmployerAccountHandler authorization failed because employerAccountClaim was null");
            }
            else
            {
                logger.LogInformation("EmployerAccountHandler authorization failed because employerAccounts didn't contain key {Key}", accountIdFromUrl);
            }
            
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