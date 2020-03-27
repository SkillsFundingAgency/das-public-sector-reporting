using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.PSRService.Web.Configuration;

namespace SFA.DAS.PSRService.Web.Filters
{
    public class ZendeskApiFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.Controller as Controller;
            if (controller != null)
            {
                var user = controller.User;
                string accountIdFromUrl = string.Empty;
                if (user.HasClaim(c => c.Type.Equals(EmployerPsrsClaims.AccountsClaimsTypeIdentifier)))
                {
                    accountIdFromUrl =
                        filterContext.RouteData.Values[RouteValues.EmployerAccountId].ToString().ToUpper();
                }
                controller.ViewBag.ZendeskApiData = new ZendeskApiData
                {
                    UserId = user.Claims.First(c => c.Type.Equals(EmployerPsrsClaims.IdamsUserIdClaimTypeIdentifier)).Value,
                    Acc = accountIdFromUrl
                };
            }

            base.OnActionExecuting(filterContext);
        }
        
        public class ZendeskApiData
        {
            public string UserId { get; set; }
            public string Acc { get; set; }
        }
    }
}