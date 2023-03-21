using System;
using System.Collections.Generic;
using System.Composition;
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
using StructureMap.Query;

namespace SFA.DAS.PSRService.Web.Controllers
{
    [Authorize]
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
        public IActionResult Edit()
        {
            var report = _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);

            if (!_reportService.CanBeEdited(report))
                return new RedirectResult(Url.Action("Index", "Home"));

            var viewModel = new ReportViewModel
            {
                Report = report,
                UserCanSubmitReports = UserIsAuthorizedForReportSubmission()
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
        [Route("IsLocalAuthority/{id}")]
        [Route("IsLocalAuthority")]
        [Authorize(Policy = PolicyNames.CanEditReport)]
        public IActionResult IsLocalAuthority(bool? id)
        {
            var report = _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);

            bool? isLocalAuthority = null;

            if (report != null)
            {
                if (!_reportService.CanBeEdited(report))
                    return new RedirectResult(Url.Action("Index", "Home"));
                isLocalAuthority = id.HasValue ? id : report.IsLocalAuthority;
                ViewBag.CurrentPeriod = report.Period ?? _currentPeriod;
            }
            else
            {
                ViewBag.CurrentPeriod = _currentPeriod;
            }

            return View("IsLocalAuthority", isLocalAuthority);
        }

        [HttpPost]
        [Route("PostIsLocalAuthority")]
        [Authorize(Policy = PolicyNames.CanEditReport)]
        public IActionResult PostIsLocalAuthority(bool? isLocalAuthority)
        {
            try
            {
                if (!isLocalAuthority.HasValue)
                {
                    ModelState.AddModelError("confirm-yes", "Select whether you are a local authority or not");
                    return View("IsLocalAuthority");
                }

                var report = _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);

                if (report == null)
                {
                    var user = _userService.GetUserModel(User);
                    _reportService.CreateReport(EmployerAccount.AccountId, user, isLocalAuthority);
                }
                else
                {
                    if (isLocalAuthority == report.IsLocalAuthority)
                        return new RedirectResult(Url.Action("Edit", "Report"));

                    if (!_reportService.CanBeEdited(report))
                        return new RedirectResult(Url.Action("Index", "Home"));

                    return new RedirectResult(Url.Action("DataLossWarning", new DataLossWarning() { IsLocalAuthority = isLocalAuthority.Value }));
                }

                return new RedirectResult(Url.Action("Edit", "Report"));
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        [HttpPost]
        [Route("DataLossWarning")]
        [Authorize(Policy = PolicyNames.CanEditReport)]
        public IActionResult PostDataLossWarning(DataLossWarning dataLossWarning)
        {
            try
            {
                var report = _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);

                if (report != null)
                {
                    if (!_reportService.CanBeEdited(report))
                        return new RedirectResult(Url.Action("Index", "Home"));

                    if (dataLossWarning.IsLocalAuthority == report.IsLocalAuthority)
                        return new RedirectResult(Url.Action("Edit", "Report"));

                    _reportService.SaveReport(report, _userService.GetUserModel(User), dataLossWarning.IsLocalAuthority, true);
                }

                return new RedirectResult(Url.Action("Edit", "Report"));
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        [HttpGet]
        [Route("DataLossWarning")]
        [Authorize(Policy = PolicyNames.CanEditReport)]
        public IActionResult DataLossWarning(DataLossWarning dataLossWarning)
        {
            return View("DataLossWarning", dataLossWarning);
        }

        [Route("List")]
        public IActionResult List([FromRoute] string hashedEmployerAccountId)
        {
            //need to get employee id, this needs to be moves somewhere

            var reportListViewmodel = new ReportListViewModel
            {
                HashedEmployerAccountId = hashedEmployerAccountId,
                SubmittedReports = _reportService.GetSubmittedReports(EmployerAccount.AccountId),
                Periods = new Dictionary<string, Period>()
            };

            foreach (var submittedReport in reportListViewmodel.SubmittedReports)
            {
                if (reportListViewmodel.Periods.ContainsKey(submittedReport.ReportingPeriod) == false)
                    reportListViewmodel.Periods.Add(submittedReport.ReportingPeriod, Period.ParsePeriodString(submittedReport.ReportingPeriod));
            }

            return View("List", reportListViewmodel);
        }

        [Route("History")]
        public IActionResult History()
        {
            var model = new ReportHistoryViewModel
            {
                Period = _currentPeriod,
            };

            model.EditHistoryMostRecentFirst =
                _reportService
                    .GetReportEditHistoryMostRecentFirst(
                        _currentPeriod,
                        EmployerAccount.AccountId);

            return View("History", model);
        }

        [Route("Summary/{period}")]
        [Route("Summary")]
        public IActionResult Summary([FromRoute] string hashedEmployerAccountId, string period)
        {
            try
            {
                if (period == null)
                    period = _currentPeriod.PeriodString;

                var report = _reportService.GetReport(period, EmployerAccount.AccountId);

                var reportViewModel = new ReportViewModel
                {
                    Report = report,
                    CurrentPeriod = _currentPeriod,
                    CanBeEdited = _reportService.CanBeEdited(report) && UserIsAuthorizedForReportEdit(),
                    UserCanEditReports = UserIsAuthorizedForReportEdit(),
                    UserCanSubmitReports = UserIsAuthorizedForReportSubmission(),
                    IsReadOnly = (UserIsAuthorizedForReportEdit() == false)
                };

                if (reportViewModel.Report != null)
                {
                    reportViewModel.IsValidForSubmission = reportViewModel.Report.IsValidForSubmission();
                    reportViewModel.Percentages = new PercentagesViewModel(reportViewModel.Report.ReportingPercentages);
                    if (reportViewModel.Report.ReportingPercentagesSchools != null) reportViewModel.PercentagesSchools = new PercentagesViewModel(reportViewModel.Report.ReportingPercentagesSchools);
                    ViewBag.CurrentPeriod = reportViewModel.Report.Period ?? _currentPeriod;
                }
                else
                {
                    reportViewModel.IsValidForSubmission = false;
                    ViewBag.CurrentPeriod = _currentPeriod;
                }

                TryValidateModel(reportViewModel);

                reportViewModel.Subtitle = GetSubtitleForUserAccessLevel();
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
        public IActionResult Confirm()
        {
            var report = _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);

            if (report == null)
                return new NotFoundResult();

            if (report.Submitted)
            {
                return new RedirectResult(Url.Action("Index", "Home"));
            }

            if (report.IsValidForSubmission() == false)
            {
                return new RedirectResult(Url.Action("Summary", "Report"));
            }

            var viewModel = new ReportViewModel { Report = report };

            if (!TryValidateModel(viewModel) || !_reportService.CanBeEdited(report))
                return new RedirectResult(Url.Action("Summary", "Report"));

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
        public IActionResult SubmitPost()
        {
            var report = _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);

            if (report == null)
                return new NotFoundResult();

            if (!TryValidateModel(new ReportViewModel { Report = report }))
                return new RedirectResult(Url.Action("Summary", "Report"));

            var user = _userService.GetUserModel(User);

            report.SubmittedDetails = new Submitted
            {
                SubmittedAt = DateTime.UtcNow,
                SubmittedEmail = user.Email,
                SubmittedName = user.DisplayName,
                SubmttedBy = user.Id.ToString(),
                UniqueReference = "NotAUniqueReference"
            };

            _reportService.SubmitReport(report);

            ViewBag.CurrentPeriod = _currentPeriod;

            return View("SubmitConfirmation");
        }

        [Route("OrganisationName")]
        [Authorize(Policy = PolicyNames.CanEditReport)]
        public IActionResult OrganisationName(string post)
        {
            var report = _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);

            if (!_reportService.CanBeEdited(report))
                return new RedirectResult(Url.Action("Index", "Home"));

            var organisationVm = new OrganisationViewModel
            {
                EmployerAccount = EmployerAccount,
                Report = report
            };

            if (string.IsNullOrEmpty(organisationVm.Report.OrganisationName))
                organisationVm.Report.OrganisationName = organisationVm.EmployerAccount.EmployerName;

            return View("OrganisationName", organisationVm);
        }

        [Route("TotalEmployees")]
        [Authorize(Policy = PolicyNames.CanEditReport)]
        public IActionResult TotalEmployees()
        {
            var report = _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);

            if (!_reportService.CanBeEdited(report))
                return new RedirectResult(Url.Action("Index", "Home"));

            ViewBag.CurrentPeriod = report?.Period ?? _currentPeriod;

            return View("TotalEmployees", report.HasMinimumEmployeeHeadcount);
        }

        [Route("PostTotalEmployees")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = PolicyNames.CanEditReport)]
        public IActionResult PostTotalEmployees(bool? hasMinimumEmployeeHeadcount)
        {
            if (hasMinimumEmployeeHeadcount == false)
                return new RedirectResult(Url.Action("ReportNotRequired"));

            var report = _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);

            if (!hasMinimumEmployeeHeadcount.HasValue)
            {
                ViewBag.CurrentPeriod = report?.Period ?? _currentPeriod;
                ModelState.AddModelError("confirm-yes", "Please select did your organisation employ more than 250 people on 31 March 2021 or not");
                return View("TotalEmployees", hasMinimumEmployeeHeadcount);
            }

            report.HasMinimumEmployeeHeadcount = hasMinimumEmployeeHeadcount;

            _reportService.SaveReport(report, _userService.GetUserModel(User), null, false);

            return new RedirectResult(Url.Action("Edit", "Report"));
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
        public IActionResult Change(OrganisationViewModel organisationVm)
        {
            var reportViewModel = new ReportViewModel
            {
                Report = _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId)
            };

            reportViewModel.Report.OrganisationName = organisationVm.Report.OrganisationName;

            _reportService.SaveReport(reportViewModel.Report, _userService.GetUserModel(User), null, false);

            return new RedirectResult(Url.Action("Edit", "Report"));
        }


        [Route("Amend")]
        [Authorize(Policy = PolicyNames.CanEditReport)]
        public IActionResult Amend()
        {
            _mediatr.Send(
                new UnSubmitReportRequest(EmployerAccount.AccountId, _currentPeriod));

            return new RedirectResult(Url.Action("Edit", "Report"));
        }

        [HttpGet]
        [Route("ConfirmAmend")]
        [Authorize(Policy = PolicyNames.CanEditReport)]
        public IActionResult ConfirmAmend([FromRoute] string hashedEmployerAccountId)
        {
            ViewBag.ReportPeriod = _currentPeriod;

            return View();
        }

        private bool UserIsAuthorizedForReportSubmission()
        {
            return
                _authorizationService
                    .AuthorizeAsync(
                        User,
                        this.ControllerContext,
                        PolicyNames.CanSubmitReport)
                    .Result
                    .Succeeded;
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

        private string GetSubtitleForUserAccessLevel()
        {
            var firstStep =
                SummaryPageMessageBuilder
                    .GetSubtitle();

            if (UserIsAuthorizedForReportSubmission())
                return
                    firstStep
                        .ForUserWhoCanSubmit();

            if (UserIsAuthorizedForReportEdit())
                return
                    firstStep
                        .ForUserWhoCanEditButNotSubmit();

            return
                firstStep
                    .ForViewOnlyUser();
        }
    }
}