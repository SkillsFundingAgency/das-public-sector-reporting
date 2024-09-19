using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Configuration.Authorization;

namespace SFA.DAS.PSRService.Web.Controllers;

[Authorize(Policy = nameof(PolicyNames.HasEmployerAccount))]
public class ServiceController(IWebConfiguration webConfiguration) : Controller
{
    public IActionResult ChangePassword(string employerId)
    {
        var baseUrl = BaseUrl();
        return Redirect($"{baseUrl}Service/Password/change");
    }
        
    public IActionResult ChangeEmail(string employerId)
    {
        var baseUrl = BaseUrl();
        return Redirect($"{baseUrl}Service/Email/change");
    }
    
    [AllowAnonymous]
    public IActionResult AccessDenied(string action)
    {
        return View("AccessDenied");
    }

    private string BaseUrl()
    {
        var baseUrl = webConfiguration.RootDomainUrl.EndsWith('/')
            ? webConfiguration.RootDomainUrl
            : webConfiguration.RootDomainUrl + "/";
        
        return baseUrl;
    }
}