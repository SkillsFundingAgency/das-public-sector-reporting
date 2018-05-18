using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PSRService.Web.Configuration;

namespace SFA.DAS.PSRService.Web.Controllers
{
    [Authorize]
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

        private string BaseUrl()
        {
            var baseUrl = _webConfiguration.RootDomainUrl.EndsWith("/")
                ? _webConfiguration.RootDomainUrl
                : _webConfiguration.RootDomainUrl + "/";
            return baseUrl;
        }
    }
}
