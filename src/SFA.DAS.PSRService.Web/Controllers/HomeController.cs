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
using SFA.DAS.PSRService.Web.DisplayText;
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
                [Home.View.SubmitValue] = Home.View,
                [Home.AlreadySubmitted.SubmitValue] = Home.AlreadySubmitted
            };
        }

        public IActionResult Index()
        {
            var model = new IndexViewModel();

            model.Period = _currentPeriod;

            var report = _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);

            PopulateModelBasedOnReportStateAndUserAuthorization(model, report);

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

        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        [Route("SignOutCleanup")]
        public async Task SignOutCleanup()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
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

        private void PopulateModelBasedOnReportStateAndUserAuthorization(IndexViewModel model, Report report)
        {
            bool reportExists = report != null;
            bool reportDoesNotExist = reportExists == false;
            bool reportIsAlreadySubmitted = report?.Submitted ?? false;
            bool reportIsNotYetSubmitted = reportIsAlreadySubmitted == false;
            bool userIsAuthorizedForReportEdit = UserIsAuthorizedForReportEdit();
            bool userIsNotAuthorizedForReportEdit = userIsAuthorizedForReportEdit == false;

            // TODO: take submission period close date into account
            model.CanCreateReport =  reportDoesNotExist && userIsAuthorizedForReportEdit;
            model.CanEditReport = reportExists && reportIsNotYetSubmitted && userIsAuthorizedForReportEdit;
            model.Readonly = userIsNotAuthorizedForReportEdit;
            model.CurrentReportAlreadySubmitted = reportIsAlreadySubmitted;

            model.WelcomeMessage = BuildWelcomeMessageFromReportStatusAndUserAuthorization(report);
        }

        private string BuildWelcomeMessageFromReportStatusAndUserAuthorization(Report report)
        {
            return

            new HomePageMessageProvider(this, _authorizationService)
                .GetWelcomeMessage()
                .ForPeriod(_currentPeriod)
                .AndReport(report);
        }
    }
}
