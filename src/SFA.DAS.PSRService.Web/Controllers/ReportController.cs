using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
using SFA.DAS.PSRService.Web.DisplayText;
using SFA.DAS.PSRService.Web.Services;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.Controllers
{
    [Authorize]
    [Route("accounts/{employerAccountId}/Report")]
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly Period _currentPeriod;

        public ReportController(
            IReportService reportService,
            IEmployerAccountService employerAccountService, 
            IUserService userService,
            IWebConfiguration webConfiguration, 
            IPeriodService periodService,
            IAuthorizationService authorizationService)
            : base(webConfiguration, employerAccountService)
        {
            _reportService = reportService;
            _userService = userService;
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
            _currentPeriod = periodService.GetCurrentPeriod();
        }

        [Authorize(Policy = PolicyNames.CanEditReport)]
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
            try
            {
                _reportService.CreateReport(EmployerAccount.AccountId);
                return new RedirectResult(Url.Action("Edit", "Report"));
            }
            catch (Exception)
            {

                return new BadRequestResult();
            }
        }

        [Route("List")]
        public IActionResult List()
        {
            //need to get employee id, this needs to be moves somewhere

            var reportListViewmodel = new ReportListViewModel
            {
                SubmittedReports = _reportService.GetSubmittedReports(EmployerAccount.AccountId),
                Periods = new Dictionary<string, Period>()
            };

            foreach (var submittedReport in reportListViewmodel.SubmittedReports)
            {
                if (reportListViewmodel.Periods.ContainsKey(submittedReport.ReportingPeriod) == false)
                    reportListViewmodel.Periods.Add(submittedReport.ReportingPeriod, new Period(submittedReport.ReportingPeriod));
            }

            return View("List", reportListViewmodel);
        }

        [Route("Summary/{period}")]
        [Route("Summary")]
        public IActionResult Summary(string period)
        {
            try
            {

                if (period == null)
                    period = _currentPeriod.PeriodString;

                var report = _reportService.GetReport(period, EmployerAccount.AccountId);

                if (report == null)
                    return new NotFoundResult();

                var reportViewModel = new ReportViewModel
                {
                    Report = report,
                    Period = _currentPeriod,
                    CanBeEdited = _reportService.CanBeEdited(report) && UserIsAuthorizedForReportEdit(),
                    UserCanSubmitReports = UserIsAuthorizedForReportSubmission(),
                    IsReadOnly = (UserIsAuthorizedForReportEdit() == false)
                };



                if (reportViewModel.Report == null)
                    return new RedirectResult(Url.Action("Index", "Home"));

                reportViewModel.IsValidForSubmission = reportViewModel.Report.IsValidForSubmission();
                reportViewModel.Percentages = new PercentagesViewModel(reportViewModel.Report?.ReportingPercentages);
                reportViewModel.Period = reportViewModel.Report?.Period;

                ViewBag.CurrentPeriod = reportViewModel.Period;

                TryValidateModel(reportViewModel);

                reportViewModel.Subtitle = GetSubtitleForUserAccessLevel();

                return View("Summary",reportViewModel);
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
                return new RedirectResult(Url.Action("Index","Home"));
            }

            if (report.IsValidForSubmission() == false)
            {
                return new RedirectResult(Url.Action("Summary", "Report"));
            }

            var viewModel = new ReportViewModel { Report = report };

            if (!TryValidateModel(viewModel) || !_reportService.CanBeEdited(report) )
                return new RedirectResult(Url.Action("Summary", "Report"));

            ViewBag.CurrentPeriod = _currentPeriod;

            return View(viewModel);
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

            return View("Submitted");
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

            _reportService.SaveReport(reportViewModel.Report);

            return new RedirectResult(Url.Action("Edit", "Report"));
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