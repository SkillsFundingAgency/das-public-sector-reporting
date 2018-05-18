using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Middleware
{
    public class EmployerAccountHandler : AuthorizationHandler<EmployerAccountRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployerAccountRequirement requirement)
        {

            if (context.Resource is AuthorizationFilterContext mvcContext && mvcContext.RouteData.Values.ContainsKey(RouteValues.EmployerAccountId))
            {
                if (context.User.HasClaim(c=>c.Type.Equals(EmployerPsrsClaims.AccountsClaimsTypeIdentifier)))
                {
                    var accountIdFromUrl = mvcContext.RouteData.Values[RouteValues.EmployerAccountId].ToString().ToUpper();
                    var employerAccountClaim = context.User.FindFirst(c=>c.Type.Equals(EmployerPsrsClaims.AccountsClaimsTypeIdentifier));
                    var employerAccounts = JsonConvert.DeserializeObject<Dictionary<string, EmployerIdentifier>>(employerAccountClaim?.Value);

                    if (employerAccountClaim != null && employerAccounts.ContainsKey(accountIdFromUrl))
                    {
                        mvcContext.HttpContext.Items.Add(ContextItemKeys.EmployerIdentifier, employerAccounts.GetValueOrDefault(accountIdFromUrl));
                        context.Succeed(requirement);
                    }
                }
                
            }
            return Task.CompletedTask;
        }
    }
}
