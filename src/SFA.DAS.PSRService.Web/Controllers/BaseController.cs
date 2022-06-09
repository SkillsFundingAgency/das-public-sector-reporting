using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly IEmployerAccountService _employerAccountService;
        protected readonly IWebConfiguration _webConfiguration;
        public EmployerIdentifier EmployerAccount => _employerAccountService.GetCurrentEmployerAccountId(HttpContext);
        protected BaseController(IWebConfiguration webConfiguration, IEmployerAccountService employerAccountService)
        {
            _webConfiguration = webConfiguration;
            _employerAccountService = employerAccountService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var urlBaseUri = new Uri(_webConfiguration.EmployerCommitmentsV2BaseUrl);
            ViewData["HomeUrl"] = new Uri(urlBaseUri, EmployerAccount?.AccountId).ToString();
            base.OnActionExecuting(context);
        }
    }
}