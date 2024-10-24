using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Filters;

public class ZenDeskApiData
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Organization { get; set; }
}

public class ZenDeskApiFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (filterContext.Controller is Controller controller)
        {
            var user = controller.User;
            EmployerIdentifier account = null;
            var accountIdFromUrl = filterContext.RouteData.Values[RouteValues.HashedEmployerAccountId]?.ToString()?.ToUpper();

            if (user.HasClaim(c => c.Type.Equals(EmployerPsrsClaims.AccountsClaimsTypeIdentifier)))
            {
                var employerAccountClaim = user.FindFirst(c => c.Type.Equals(EmployerPsrsClaims.AccountsClaimsTypeIdentifier));
                var employerAccounts = JsonConvert.DeserializeObject<Dictionary<string, EmployerIdentifier>>(employerAccountClaim?.Value);
                
                if (accountIdFromUrl != null)
                {
                    account = employerAccounts[accountIdFromUrl];
                }
            }

            controller.ViewBag.ZendeskApiData = new ZenDeskApiData
            {
                Name = user.Claims.FirstOrDefault(c => c.Type.Equals(EmployerPsrsClaims.NameClaimsTypeIdentifier))?.Value,
                Email = user.Claims.FirstOrDefault(c => c.Type.Equals(EmployerPsrsClaims.EmailClaimsTypeIdentifier))?.Value,
                Organization = account?.EmployerName
            };
        }

        base.OnActionExecuting(filterContext);
    }
}