using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
using SFA.DAS.PSRService.Web.Services;
using SFA.DAS.PSRService.Web.SubmitActions;
using SFA.DAS.PSRService.Web.ViewModels;
using SFA.DAS.PSRService.Web.ViewModels.Home;

namespace SFA.DAS.PSRService.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IReportService _reportService;
        private readonly IPeriodService _periodService;
        private readonly IAuthorizationService _authorizationService;

        private readonly Period _currentPeriod;

        private readonly IReadOnlyDictionary<string, SubmitAction> submitLookup;

        public HomeController(IReportService reportService, IEmployerAccountService employerAccountService,
            IWebConfiguration webConfiguration, IPeriodService periodService,
            IAuthorizationService authorizationService)
            : base(webConfiguration, employerAccountService)
        {
            _reportService = reportService;
            _periodService = periodService;
            _authorizationService = authorizationService;

            _currentPeriod = _periodService.GetCurrentPeriod();

            submitLookup = new ReadOnlyDictionary<string, SubmitAction>(
                buildSubmitLookups());
        }

        private IDictionary<string, SubmitAction> buildSubmitLookups()
        {
            return new Dictionary<string, SubmitAction>
            {
                [Home.Edit.SubmitValue] = Home.Edit,
                [Home.List.SubmitValue] = Home.List,
                [Home.Create.SubmitValue] = Home.Create,
                [Home.View.SubmitValue] = Home.View
            };
        }

        public IActionResult Index()
        {
            var model = new IndexViewModel();

            var report = _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);
            model.Period = _currentPeriod;
            model.CurrentReportExists = report != null;
            // TODO: take submission period close date into account
            model.CanCreateReport = report == null && UserIsAuthorizedForReportEdit();
            model.CanEditReport = report != null && !report.Submitted && UserIsAuthorizedForReportEdit();
            model.Readonly = UserIsAuthorizedForReportEdit() == false;
            model.CurrentReportAlreadySubmitted = report?.Submitted ?? false;
            return View(model);
        }

        public IActionResult Submit(string action)
        {
            if (submitLookup.ContainsKey(action))
                return
                    BuildRedirectResultForSubmitAction(
                        submitLookup[action]);

            return
                new BadRequestResult();
        }

        private RedirectResult BuildRedirectResultForSubmitAction(SubmitAction submitAction)
        {
            return
                new RedirectResult(
                    Url.Action(
                        submitAction.ActionName,
                        submitAction.ControllerName));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

            var disco = await DiscoveryClient.GetAsync(_webConfiguration.Identity.Authority);
            return Redirect(disco.EndSessionEndpoint);
        }

        private bool UserIsAuthorizedForReportEdit()
        {
            return
                _authorizationService
                    .AuthorizeAsync(
                        User,
                        this.ControllerContext,
                        PolicyNames.CanEditReport)
                    .Result
                    .Succeeded;
        }
    }
}
