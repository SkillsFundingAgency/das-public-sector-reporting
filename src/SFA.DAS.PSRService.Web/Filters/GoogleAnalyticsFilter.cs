using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.PSRService.Web.Configuration;

namespace SFA.DAS.PSRService.Web.Filters
{
    public class GoogleAnalyticsFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.Controller as Controller;
            if (controller != null)
            {
                var user = controller.User;
                string accountIdFromUrl = filterContext.RouteData.Values[RouteValues.EmployerAccountId]?.ToString().ToUpper(); ;
                controller.ViewBag.GaData = new GaData
                {
                    UserId = user.Claims.First(c => c.Type.Equals(EmployerPsrsClaims.IdamsUserIdClaimTypeIdentifier)).Value,
                    Acc = accountIdFromUrl
                };
            }

            base.OnActionExecuting(filterContext);
        }

        public class GaData
        {
            public string UserId { get; set; }
            public string Vpv { get; set; }
            public string Acc { get; set; }
        }
    }
}