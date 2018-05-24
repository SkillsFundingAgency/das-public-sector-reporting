using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Configuration;
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
        private readonly Period _currentPeriod;

        public ReportController(IReportService reportService, IEmployerAccountService employerAccountService, IUserService userService, IWebConfiguration webConfiguration, IPeriodService periodService)
            : base(webConfiguration, employerAccountService)
        {
            _reportService = reportService;
            _userService = userService;
            _currentPeriod = periodService.GetCurrentPeriod();
        }

        public IActionResult Edit()
        {
            var report = _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);

            if (!_reportService.CanBeEdited(report))
                return new RedirectResult(Url.Action("Index", "Home"));

            return View("Edit", new ReportViewModel {Report = report});
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            ViewBag.CurrentPeriod = _currentPeriod;
            return View("Create");
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult PostCreate()
        {
            try
            {
                var user = _userService.GetUserModel(User);
                _reportService.CreateReport(EmployerAccount.AccountId, user);
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

        [Route("AuditHistory")]
        public IActionResult AuditHistory()
        {
            _reportService.
            return View();
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
                    CanBeEdited = _reportService.CanBeEdited(report)
                };

                if (reportViewModel.Report == null)
                    return new RedirectResult(Url.Action("Index", "Home"));

                reportViewModel.IsValidForSubmission = reportViewModel.Report.IsValidForSubmission();
                reportViewModel.Percentages = new PercentagesViewModel(reportViewModel.Report?.ReportingPercentages);
                reportViewModel.Period = reportViewModel.Report?.Period;

                ViewBag.CurrentPeriod = reportViewModel.Period;

                TryValidateModel(reportViewModel);

                return View(reportViewModel);
            }
            catch
            {
                return new BadRequestResult();
            }
        }

        [HttpGet]
        [Route("Confirm")]
        public IActionResult Confirm()
        {
            var report = _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);

            if (report == null)
                return new NotFoundResult();

            var viewModel = new ReportViewModel { Report = report };

            if (!TryValidateModel(viewModel) || !_reportService.CanBeEdited(report) || !report.IsValidForSubmission())
                return new RedirectResult(Url.Action("Summary", "Report"));

            ViewBag.CurrentPeriod = _currentPeriod;

            return View(viewModel);
        }

        [HttpPost]
        [Route("Submit")]
        public IActionResult Submit()
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



        //[Route("accounts/{employerAccountId}/[controller]/OrganisationName")]
        [Route("OrganisationName")]
        public IActionResult OrganisationName(string post)
        {
            var organisationVm = new OrganisationViewModel
            {
                EmployerAccount = EmployerAccount,
                Report = _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId)
            };

            if (string.IsNullOrEmpty(organisationVm.Report.OrganisationName))
                organisationVm.Report.OrganisationName = organisationVm.EmployerAccount.EmployerName;

            return View("OrganisationName", organisationVm);
        }

        //[Route("accounts/{employerAccountId}/[controller]/Change")]
        [Route("Change")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Change(OrganisationViewModel organisationVm)
        {
            var reportViewModel = new ReportViewModel
            {
                Report = _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId)
            };

            reportViewModel.Report.OrganisationName = organisationVm.Report.OrganisationName;

            _reportService.SaveReport(reportViewModel.Report, _userService.GetUserModel(User));

            return new RedirectResult(Url.Action("Edit", "Report"));
        }
    }
}