using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.HasEmployerAccount))]
    public class ServiceController : Controller
    {
        private readonly IWebConfiguration _webConfiguration;

        public ServiceController(IWebConfiguration webConfiguration)
        {
            _webConfiguration = webConfiguration;
        }
        
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
            var baseUrl = _webConfiguration.RootDomainUrl.EndsWith("/")
                ? _webConfiguration.RootDomainUrl
                : _webConfiguration.RootDomainUrl + "/";
            return baseUrl;
        }
    }
}
