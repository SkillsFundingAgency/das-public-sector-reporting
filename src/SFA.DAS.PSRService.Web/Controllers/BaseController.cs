using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.Controllers;

public abstract class BaseController(IWebConfiguration webConfiguration, IEmployerAccountService employerAccountService) : Controller
{
    protected EmployerIdentifier EmployerAccount => employerAccountService.GetCurrentEmployerAccountId(HttpContext);

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var urlBaseUri = new Uri(webConfiguration.EmployerCommitmentsV2BaseUrl);
        ViewData["HomeUrl"] = new Uri(urlBaseUri, EmployerAccount?.AccountId).ToString();
        ViewData[RouteValues.HashedEmployerAccountId] = EmployerAccount?.AccountId;
        base.OnActionExecuting(context);
    }
}