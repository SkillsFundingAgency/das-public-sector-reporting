using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Middleware.Authorization
{
    public class EmployerAccountHandler : AuthorizationHandler<EmployerAccountRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployerAccountHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployerAccountRequirement requirement)
        {
            if (!_httpContextAccessor.HttpContext.Request.RouteValues.ContainsKey(RouteValues.HashedEmployerAccountId))
            {
                return;
            }
            
            var accountIdFromUrl = _httpContextAccessor.HttpContext.Request.RouteValues[RouteValues.HashedEmployerAccountId].ToString().ToUpper();
            
            if (context.User.HasClaim(c => c.Type.Equals(EmployerPsrsClaims.AccountsClaimsTypeIdentifier)))
            {
                var employerAccountClaim = context.User.FindFirst(c => c.Type.Equals(EmployerPsrsClaims.AccountsClaimsTypeIdentifier));
                var employerAccounts = JsonConvert.DeserializeObject<Dictionary<string, EmployerIdentifier>>(employerAccountClaim?.Value);

                if (employerAccountClaim != null && employerAccounts.ContainsKey(accountIdFromUrl))
                {
                    if (context.Resource is AuthorizationFilterContext mvcContext && !mvcContext.HttpContext.Items.ContainsKey(ContextItemKeys.EmployerIdentifier))
                    {
                        mvcContext.HttpContext.Items.Add(ContextItemKeys.EmployerIdentifier, employerAccounts.GetValueOrDefault(accountIdFromUrl));
                    }
                    
                    context.Succeed(requirement);
                }
            }
        }
    }
}
