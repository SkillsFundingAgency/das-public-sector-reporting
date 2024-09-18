using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
using SFA.DAS.PSRService.Web.DisplayText;
using SFA.DAS.PSRService.Web.Services;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.Controllers;

[Authorize(Policy = nameof(PolicyNames.HasEmployerAccount))]
[Route("accounts/{hashedEmployerAccountId}/[controller]")]
public class ReportController : BaseController
{
    private readonly IReportService _reportService;
    private readonly IUserService _userService;
    private readonly IAuthorizationService _authorizationService;
    private readonly IMediator _mediatr;
    private readonly Period _currentPeriod;

    public ReportController(IReportService reportService,
        IEmployerAccountService employerAccountService,
        IUserService userService,
        IWebConfiguration webConfiguration,
        IPeriodService periodService,
        IAuthorizationService authorizationService,
        IMediator mediatr)
        : base(webConfiguration, employerAccountService)
    {
        _reportService = reportService;
        _userService = userService;
        _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
        _mediatr = mediatr ?? throw new ArgumentNullException(nameof(mediatr));
        _currentPeriod = periodService.GetCurrentPeriod();
    }

    [Route("AlreadySubmitted")]
    public ViewResult AlreadySubmitted()
    {
        ViewBag.CurrentPeriod = _currentPeriod;

        return View("AlreadySubmitted");
    }

    [Authorize(Policy = PolicyNames.CanEditReport)]
    [Route("Edit")]
    public async Task<IActionResult> Edit()
    {
        var report = await _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);

        if (!_reportService.CanBeEdited(report))
        {
            return RedirectToAction("Index", "Home");
        }

        var viewModel = new ReportViewModel
        {
            Report = report,
            UserCanSubmitReports = await UserIsAuthorizedForReportSubmission()
        };

        return View("Edit", viewModel);
    }

    [HttpGet]
    [Route("Create")]
    [Authorize(Policy = PolicyNames.CanEditReport)]
    public IActionResult Create()
    {
        ViewBag.CurrentPeriod = _currentPeriod;
        return View("Create");
    }

    [HttpPost]
    [Route("Create")]
    [Authorize(Policy = PolicyNames.CanEditReport)]
    public IActionResult PostCreate()
    {
        return RedirectToAction("IsLocalAuthority", "Report");
    }

    [HttpGet]
    [Route("IsLocalAuthority")]
    [Authorize(Policy = PolicyNames.CanEditReport)]
    public async Task<IActionResult> IsLocalAuthority(bool? confirmIsLocalAuthority)
    {
        var report = await _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);

        var isLocalAuthorityViewModelVm = new IsLocalAuthorityViewModel();

        if (report != null)
        {
            if (!_reportService.CanBeEdited(report))
            {
                return RedirectToAction("Index", "Home");
            }

            isLocalAuthorityViewModelVm.IsLocalAuthority = confirmIsLocalAuthority.HasValue ? confirmIsLocalAuthority : report.IsLocalAuthority;
            ViewBag.CurrentPeriod = report.Period ?? _currentPeriod;
        }
        else
        {
            ViewBag.CurrentPeriod = _currentPeriod;
        }

        return View("IsLocalAuthority", isLocalAuthorityViewModelVm);
    }

    [HttpPost]
    [Route("IsLocalAuthority")]
    [Authorize(Policy = PolicyNames.CanEditReport)]
    public async Task<IActionResult> PostIsLocalAuthority(IsLocalAuthorityViewModel isLocalAuthorityViewModel)
    {
        try
        {
            if (!isLocalAuthorityViewModel.IsLocalAuthority.HasValue)
            {
                ModelState.AddModelError("confirm-yes", "Select 'yes' if your organisation is a local authority");
                return View("IsLocalAuthority", isLocalAuthorityViewModel);
            }

            var report = await _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);

            if (report == null)
            {
                var user = _userService.GetUserModel(User);
                await _reportService.CreateReport(EmployerAccount.AccountId, user, isLocalAuthorityViewModel.IsLocalAuthority);
            }
            else
            {
                if (isLocalAuthorityViewModel.IsLocalAuthority == report.IsLocalAuthority)
                {
                    return RedirectToAction("Edit", "Report");
                }

                if (!_reportService.CanBeEdited(report))
                {
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("DataLossWarning", new DataLossWarningViewModel { IsLocalAuthority = isLocalAuthorityViewModel.IsLocalAuthority.Value });
            }

            return RedirectToAction("Edit", "Report");
        }
        catch (Exception)
        {
            return new BadRequestResult();
        }
    }

    [HttpPost]
    [Route("DataLossWarning")]
    [Authorize(Policy = PolicyNames.CanEditReport)]
    public async Task<IActionResult> PostDataLossWarning(DataLossWarningViewModel dataLossWarning)
    {
        try
        {
            if (!dataLossWarning.ConfirmIsLocalAuthority.HasValue)
            {
                ModelState.AddModelError("confirm-yes", "Confirm,do you want to change your answer");
                return View("DataLossWarning", dataLossWarning);
            }

            if (!dataLossWarning.ConfirmIsLocalAuthority.Value)
            {
                return RedirectToAction("Edit", "Report");
            }

            var report = await _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);

            if (report == null)
            {
                return RedirectToAction("Edit", "Report");
            }

            if (!_reportService.CanBeEdited(report))
            {
                return RedirectToAction("Index", "Home");
            }

            if (dataLossWarning.IsLocalAuthority == report.IsLocalAuthority)
            {
                return RedirectToAction("Edit", "Report");
            }

            await _reportService.SaveReport(report, _userService.GetUserModel(User), dataLossWarning.IsLocalAuthority);

            return RedirectToAction("Edit", "Report");
        }
        catch (Exception)
        {
            return new BadRequestResult();
        }
    }

    [HttpGet]
    [Route("DataLossWarning")]
    [Authorize(Policy = PolicyNames.CanEditReport)]
    public IActionResult DataLossWarning(DataLossWarningViewModel dataLossWarning)
    {
        return View(dataLossWarning);
    }

    [Route("List")]
    public async Task<IActionResult> List([FromRoute] string hashedEmployerAccountId)
    {
        var reportListViewmodel = new ReportListViewModel
        {
            HashedEmployerAccountId = hashedEmployerAccountId,
            SubmittedReports = await _reportService.GetSubmittedReports(EmployerAccount.AccountId),
            Periods = new Dictionary<string, Period>()
        };

        foreach (var submittedReport in reportListViewmodel.SubmittedReports)
        {
            if (reportListViewmodel.Periods.ContainsKey(submittedReport.ReportingPeriod) == false)
            {
                reportListViewmodel.Periods.Add(submittedReport.ReportingPeriod, Period.ParsePeriodString(submittedReport.ReportingPeriod));
            }
        }

        return View("List", reportListViewmodel);
    }

    [Route("History")]
    public async Task<IActionResult> History()
    {
        var model = new ReportHistoryViewModel
        {
            Period = _currentPeriod,
        };

        model.EditHistoryMostRecentFirst = await _reportService.GetReportEditHistoryMostRecentFirst(_currentPeriod, EmployerAccount.AccountId);

        return View("History", model);
    }

    [Route("Summary/{period}")]
    [Route("Summary")]
    public async Task<IActionResult> Summary([FromRoute] string hashedEmployerAccountId, string period)
    {
        try
        {
            if (period == null)
            {
                period = _currentPeriod.PeriodString;
            }

            var report = await _reportService.GetReport(period, EmployerAccount.AccountId);

            var reportViewModel = new ReportViewModel
            {
                Report = report,
                CurrentPeriod = _currentPeriod,
                CanBeEdited = _reportService.CanBeEdited(report) && await UserIsAuthorizedForReportEdit(),
                UserCanEditReports = await UserIsAuthorizedForReportEdit(),
                UserCanSubmitReports = await UserIsAuthorizedForReportSubmission(),
                IsReadOnly = await UserIsAuthorizedForReportEdit() == false
            };

            if (reportViewModel.Report != null)
            {
                reportViewModel.IsValidForSubmission = reportViewModel.Report.IsValidForSubmission();
                reportViewModel.Percentages = new PercentagesViewModel(reportViewModel.Report.ReportingPercentages);
                if (reportViewModel.Report.ReportingPercentagesSchools != null)
                {
                    reportViewModel.PercentagesSchools = new PercentagesViewModel(reportViewModel.Report.ReportingPercentagesSchools);
                }

                ViewBag.CurrentPeriod = reportViewModel.Report.Period ?? _currentPeriod;
            }
            else
            {
                reportViewModel.IsValidForSubmission = false;
                ViewBag.CurrentPeriod = _currentPeriod;
            }

            TryValidateModel(reportViewModel);

            reportViewModel.Subtitle = await GetSubtitleForUserAccessLevel();
            reportViewModel.HashedEmployerAccountId = hashedEmployerAccountId;

            return View("Summary", reportViewModel);
        }
        catch
        {
            return new BadRequestResult();
        }
    }

    [HttpGet]
    [Route("Confirm")]
    [Authorize(Policy = PolicyNames.CanSubmitReport)]
    public async Task<IActionResult> Confirm()
    {
        var report = await _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);

        if (report == null)
        {
            return new NotFoundResult();
        }

        if (report.Submitted)
        {
            return RedirectToAction("Index", "Home");
        }

        if (report.IsValidForSubmission() == false)
        {
            return RedirectToAction("Summary", "Report");
        }

        var viewModel = new ReportViewModel { Report = report };

        if (!TryValidateModel(viewModel) || !_reportService.CanBeEdited(report))
        {
            return RedirectToAction("Summary", "Report");
        }

        ViewBag.CurrentPeriod = _currentPeriod;

        return View(viewModel);
    }

    [HttpGet]
    [Route("EditComplete")]
    [Authorize(Policy = PolicyNames.CanEditReport)]
    public IActionResult EditComplete()
    {
        return View("EditComplete");
    }

    [Route("Submit")]
    [HttpGet]
    public IActionResult Submit()
    {
        return RedirectToActionPermanent("Index", "Home");
    }

    [HttpPost]
    [Route("Submit")]
    [Authorize(Policy = PolicyNames.CanSubmitReport)]
    public async Task<IActionResult> SubmitPost()
    {
        var report = await _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);

        if (report == null)
        {
            return new NotFoundResult();
        }

        if (!TryValidateModel(new ReportViewModel { Report = report }))
        {
            return RedirectToAction("Summary", "Report");
        }

        var user = _userService.GetUserModel(User);

        report.SubmittedDetails = new Submitted
        {
            SubmittedAt = DateTime.UtcNow,
            SubmittedEmail = user.Email,
            SubmittedName = user.DisplayName,
            SubmttedBy = user.Id.ToString(),
            UniqueReference = "NotAUniqueReference"
        };

        await _reportService.SubmitReport(report);

        ViewBag.CurrentPeriod = _currentPeriod;

        return View("SubmitConfirmation");
    }

    [Route("OrganisationName")]
    [Authorize(Policy = PolicyNames.CanEditReport)]
    public async Task<IActionResult> OrganisationName(string post)
    {
        var report = await _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);

        if (!_reportService.CanBeEdited(report))
        {
            return RedirectToAction("Index", "Home");
        }

        var organisationVm = new OrganisationViewModel
        {
            EmployerAccount = EmployerAccount,
            Report = report
        };

        if (string.IsNullOrEmpty(organisationVm.Report.OrganisationName))
        {
            organisationVm.Report.OrganisationName = organisationVm.EmployerAccount.EmployerName;
        }

        return View("OrganisationName", organisationVm);
    }

    [Route("TotalEmployees")]
    [Authorize(Policy = PolicyNames.CanEditReport)]
    public async Task<IActionResult> TotalEmployees(bool? totalEmployeesConfirmation)
    {
        var report = await _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);

        if (!_reportService.CanBeEdited(report))
        {
            return RedirectToAction("Index", "Home");
        }

        ViewBag.CurrentPeriod = report?.Period ?? _currentPeriod;

        report.HasMinimumEmployeeHeadcount = totalEmployeesConfirmation.HasValue ? totalEmployeesConfirmation : report.HasMinimumEmployeeHeadcount;

        return View("TotalEmployees", report.HasMinimumEmployeeHeadcount);
    }

    [Route("TotalEmployees")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = PolicyNames.CanEditReport)]
    public async Task<IActionResult> PostTotalEmployees(bool? hasMinimumEmployeeHeadcount)
    {
        if (hasMinimumEmployeeHeadcount == false)
        {
            return RedirectToAction("ReportNotRequired");
        }

        var report = await _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);

        if (!hasMinimumEmployeeHeadcount.HasValue)
        {
            return RedirectToAction("Edit", "Report");
        }

        report.HasMinimumEmployeeHeadcount = hasMinimumEmployeeHeadcount;

        await _reportService.SaveReport(report, _userService.GetUserModel(User), null);

        return RedirectToAction("Edit", "Report");
    }

    [Route("TotalEmployeesConfirmationRequired")]
    public IActionResult TotalEmployeesConfirmationRequired()
    {
        return View();
    }

    [Route("ReportNotRequired")]
    public IActionResult ReportNotRequired()
    {
        return View();
    }

    [Route("Change")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = PolicyNames.CanEditReport)]
    public async Task<IActionResult> Change(OrganisationViewModel organisationVm)
    {
        var reportViewModel = new ReportViewModel
        {
            Report = await _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId)
        };

        reportViewModel.Report.OrganisationName = organisationVm.Report.OrganisationName;

        await _reportService.SaveReport(reportViewModel.Report, _userService.GetUserModel(User), null);

        return RedirectToAction("Edit", "Report");
    }


    [Route("Amend")]
    [Authorize(Policy = PolicyNames.CanEditReport)]
    public async Task<IActionResult> Amend()
    {
        await _mediatr.Send(new UnSubmitReportRequest(EmployerAccount.AccountId, _currentPeriod));

        return RedirectToAction("Edit", "Report");
    }

    [HttpGet]
    [Route("ConfirmAmend")]
    [Authorize(Policy = PolicyNames.CanEditReport)]
    public IActionResult ConfirmAmend([FromRoute] string hashedEmployerAccountId)
    {
        ViewBag.ReportPeriod = _currentPeriod;

        return View();
    }

    private async Task<bool> UserIsAuthorizedForReportSubmission()
    {
        var result = await _authorizationService.AuthorizeAsync(User, ControllerContext, PolicyNames.CanSubmitReport);
        return result.Succeeded;
    }

    private async Task<bool> UserIsAuthorizedForReportEdit()
    {
        var result = await _authorizationService.AuthorizeAsync(User, ControllerContext, PolicyNames.CanEditReport);
        return result.Succeeded;
    }

    private async Task<string> GetSubtitleForUserAccessLevel()
    {
        var firstStep = SummaryPageMessageBuilder.GetSubtitle();

        if (await UserIsAuthorizedForReportSubmission())
        {
            return firstStep.ForUserWhoCanSubmit();
        }

        if (await UserIsAuthorizedForReportEdit())
        {
            return firstStep.ForUserWhoCanEditButNotSubmit();
        }

        return firstStep.ForViewOnlyUser();
    }
}