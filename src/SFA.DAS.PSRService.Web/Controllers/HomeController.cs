using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SFA.DAS.GovUK.Auth.Models;
using SFA.DAS.GovUK.Auth.Services;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
using SFA.DAS.PSRService.Web.DisplayText;
using SFA.DAS.PSRService.Web.Services;
using SFA.DAS.PSRService.Web.SubmitActions;
using SFA.DAS.PSRService.Web.ViewModels;
using SFA.DAS.PSRService.Web.ViewModels.Home;

namespace SFA.DAS.PSRService.Web.Controllers;

[Authorize]
public class HomeController : BaseController
{
    private readonly IReportService _reportService;
    private readonly IPeriodService _periodService;
    private readonly IAuthorizationService _authorizationService;
    private readonly IConfiguration _config;
    private readonly IStubAuthenticationService _stubAuthenticationService;

    private readonly Period _currentPeriod;

    private readonly IReadOnlyDictionary<string, SubmitAction> _submitLookup;

    public HomeController(IReportService reportService, IEmployerAccountService employerAccountService,
        IWebConfiguration webConfiguration, IPeriodService periodService,
        IAuthorizationService authorizationService, IConfiguration config, IStubAuthenticationService stubAuthenticationService)
        : base(webConfiguration, employerAccountService)
    {
        _reportService = reportService;
        _periodService = periodService;
        _authorizationService = authorizationService;
        _config = config;
        _stubAuthenticationService = stubAuthenticationService;

        _currentPeriod = _periodService.GetCurrentPeriod();

        _submitLookup = new ReadOnlyDictionary<string, SubmitAction>(BuildSubmitLookups());
    }

    private static IDictionary<string, SubmitAction> BuildSubmitLookups()
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

    [Authorize(Policy = nameof(PolicyNames.HasEmployerAccount))]
    public async Task<IActionResult> Index()
    {
        var model = new IndexViewModel { Period = _currentPeriod };

        var report = await _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);

        await PopulateModelBasedOnReportStateAndUserAuthorization(model, report);

        return View(model);
    }

    [Authorize(Policy = nameof(PolicyNames.HasEmployerAccount))]
    public IActionResult Submit(string action)
    {
        if (_submitLookup.ContainsKey(action))
        {
            return BuildRedirectResultForSubmitAction(_submitLookup[action]);
        }

        return new BadRequestResult();
    }

#if DEBUG
    [AllowAnonymous()]
    [HttpGet]
    [Route("SignIn-Stub")]
    public IActionResult SigninStub()
    {
        return View("SigninStub", new List<string> { _config["StubId"], _config["StubEmail"] });
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("SignIn-Stub")]
    public async Task<IActionResult> SigninStubPost()
    {
        var claims = await _stubAuthenticationService.GetStubSignInClaims(new StubAuthUserDetails
        {
            Email = _config["StubEmail"],
            Id = _config["StubId"]
        });

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claims, new AuthenticationProperties());

        return RedirectToRoute("Signed-in-stub");
    }

    [Authorize(Policy = "StubAuth")]
    [HttpGet]
    [Route("signed-in-stub", Name = "Signed-in-stub")]
    public IActionResult SignedInStub()
    {
        return View();
    }
#endif

    private IActionResult BuildRedirectResultForSubmitAction(SubmitAction submitAction)
    {
        return RedirectToAction(submitAction.ActionName, submitAction.ControllerName);
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Route("SignOut", Name = "SignOut")]
    public async Task<IActionResult> Logout()
    {
        var idToken = await HttpContext.GetTokenAsync("id_token");

        var authenticationProperties = new AuthenticationProperties();
        authenticationProperties.Parameters.Clear();
        authenticationProperties.Parameters.Add("id_token", idToken);
        var schemes = new List<string>
        {
            CookieAuthenticationDefaults.AuthenticationScheme
        };
        _ = bool.TryParse(_config["StubAuth"], out var stubAuth);
        if (!stubAuth)
        {
            schemes.Add(OpenIdConnectDefaults.AuthenticationScheme);
        }

        return SignOut(authenticationProperties, schemes.ToArray());
    }

    [Route("SignOutCleanup")]
    public async Task SignOutCleanup()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    private async Task<bool> UserIsAuthorizedForReportEdit()
    {
        var result = await _authorizationService.AuthorizeAsync(User, ControllerContext, PolicyNames.CanEditReport);

        return result.Succeeded;
    }

    private async Task PopulateModelBasedOnReportStateAndUserAuthorization(IndexViewModel model, Report report)
    {
        var reportExists = report != null;
        var reportDoesNotExist = reportExists == false;
        var reportIsAlreadySubmitted = report?.Submitted ?? false;
        var reportIsNotYetSubmitted = reportIsAlreadySubmitted == false;
        var userIsAuthorizedForReportEdit = await UserIsAuthorizedForReportEdit();
        var userIsNotAuthorizedForReportEdit = userIsAuthorizedForReportEdit == false;

        // TODO: take submission period close date into account
        model.CanCreateReport = reportDoesNotExist && userIsAuthorizedForReportEdit;
        model.CanEditReport = reportExists && reportIsNotYetSubmitted && userIsAuthorizedForReportEdit;
        model.Readonly = userIsNotAuthorizedForReportEdit;
        model.CurrentReportAlreadySubmitted = reportIsAlreadySubmitted;

        model.WelcomeMessage = BuildWelcomeMessageFromReportStatusAndUserAuthorization(report);
    }

    private string BuildWelcomeMessageFromReportStatusAndUserAuthorization(Report report)
    {
        return new HomePageMessageProvider(this, _authorizationService)
                .GetWelcomeMessage()
                .ForPeriod(_currentPeriod)
                .AndReport(report);
    }
}