﻿using System.Diagnostics;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebSockets.Internal;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Models.Home;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IEmployerAccountService _employerAccountService;
        private readonly IWebConfiguration _webConfiguration;
        //private const string EmployerId = "ABCDE"; // TODO: get this from context

        private EmployerIdentifier EmployerAccount => _employerAccountService.GetCurrentEmployerAccountId(HttpContext);

        public HomeController(IReportService reportService, IEmployerAccountService employerAccountService, IWebConfiguration webConfiguration)
        {
            _reportService = reportService;
            _employerAccountService = employerAccountService;
            _webConfiguration = webConfiguration;
        }

        public IActionResult Index()
        {
            var model = new IndexViewModel();
            var period = _reportService.GetCurrentReportPeriod();
       
            var report = _reportService.GetReport(period, EmployerAccount.AccountId);
            model.PeriodName = _reportService.GetCurrentReportPeriodName(period);
            model.CanCreateReport = report == null;
            model.CanEditReport = report != null && !report.Submitted;
            return View(model);
        }

        public IActionResult Submit(string action)
        {
            if (action == "create")
                return new RedirectResult(Url.Action("Create", "Report"));
            if (action == "edit")
                return new RedirectResult(Url.Action("Edit", "Report"));

            if (action == "list")
                return new RedirectResult(Url.Action("List", "Report"));

            return new BadRequestResult();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

            var disco = await DiscoveryClient.GetAsync(_webConfiguration.Identity.Authority);
            return Redirect(disco.EndSessionEndpoint);
        }

        [Authorize]
        public IActionResult Protected(string empolyerId)
        {
            var employerDetail = (EmployerIdentifier)HttpContext.Items[ContextItemKeys.EmployerIdentifier];
            return View(employerDetail);
        }
    }
}
