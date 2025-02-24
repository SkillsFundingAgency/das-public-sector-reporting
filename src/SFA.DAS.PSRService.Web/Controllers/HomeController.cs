using System.Collections.ObjectModel;
using System.Diagnostics;
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
    private readonly IAuthorizationService _authorizationService;
    private readonly IConfiguration _config;
    private readonly IStubAuthenticationService _stubAuthenticationService;

    private readonly Period _currentPeriod;

    private readonly ReadOnlyDictionary<string, SubmitAction> _submitLookup;

    public HomeController(IReportService reportService, 
        IEmployerAccountService employerAccountService,
        IWebConfiguration webConfiguration,
        IPeriodService periodService,
        IAuthorizationService authorizationService,
        IConfiguration config,
        IStubAuthenticationService stubAuthenticationService)
        : base(webConfiguration, employerAccountService)
    {
        _reportService = reportService;
        _authorizationService = authorizationService;
        _config = config;
        _stubAuthenticationService = stubAuthenticationService;
        
        _currentPeriod = periodService.GetCurrentPeriod();
        _submitLookup = BuildSubmitLookups();
    }

    private static ReadOnlyDictionary<string, SubmitAction> BuildSubmitLookups()
    {
        var values = new Dictionary<string, SubmitAction>
        {
            [Home.Edit.SubmitValue] = Home.Edit,
            [Home.List.SubmitValue] = Home.List,
            [Home.Create.SubmitValue] = Home.Create,
            [Home.View.SubmitValue] = Home.View,
            [Home.AlreadySubmitted.SubmitValue] = Home.AlreadySubmitted
        };

        return values.AsReadOnly();
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
        return _submitLookup.TryGetValue(action, out var value)
            ? BuildRedirectResultForSubmitAction(value)
            : new BadRequestResult();
    }

#if DEBUG
    [AllowAnonymous]
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

    private RedirectToActionResult BuildRedirectResultForSubmitAction(SubmitAction submitAction)
    {
        return RedirectToAction(submitAction.ActionName, submitAction.ControllerName);
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Route("SignOut", Name = "SignOut")]
    [Route("service/signout")]
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
        var reportDoesNotExist = !reportExists;
        var reportIsAlreadySubmitted = report?.Submitted ?? false;
        var reportIsNotYetSubmitted = !reportIsAlreadySubmitted;
        var userIsAuthorizedForReportEdit = await UserIsAuthorizedForReportEdit();
        var userIsNotAuthorizedForReportEdit = !userIsAuthorizedForReportEdit;

        // TODO: take submission period close date into account
        model.CanCreateReport = reportDoesNotExist && userIsAuthorizedForReportEdit;
        model.CanEditReport = reportExists && reportIsNotYetSubmitted && userIsAuthorizedForReportEdit;
        model.Readonly = userIsNotAuthorizedForReportEdit;
        model.CurrentReportAlreadySubmitted = reportIsAlreadySubmitted;

        model.WelcomeMessage = await BuildWelcomeMessageFromReportStatusAndUserAuthorization(report);
    }

    private async Task<string> BuildWelcomeMessageFromReportStatusAndUserAuthorization(Report report)
    {
        return await new HomePageMessageProvider(this, _authorizationService)
            .GetWelcomeMessage()
            .ForPeriod(_currentPeriod)
            .AndReport(report);
    }
}