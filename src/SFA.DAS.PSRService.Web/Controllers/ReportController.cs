using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
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

        public ReportController(
            IReportService reportService,
            IEmployerAccountService employerAccountService, 
            IUserService userService,
            IWebConfiguration webConfiguration, 
            IPeriodService periodService)
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

            var reportListViewmodel = new ReportListViewModel();

            reportListViewmodel.SubmittedReports = _reportService.GetSubmittedReports(EmployerAccount.AccountId);

            reportListViewmodel.Periods = new Dictionary<string, Period>();

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
                    CanBeEdited = _reportService.CanBeEdited(report)
                };

                if (reportViewModel.Report == null)
                    return new RedirectResult(Url.Action("Index", "Home"));

                reportViewModel.IsValidForSubmission = reportViewModel.Report.IsValidForSubmission();
                reportViewModel.Percentages = new PercentagesViewModel(reportViewModel.Report?.ReportingPercentages);
                reportViewModel.Period = reportViewModel.Report?.Period;

                ViewBag.CurrentPeriod = reportViewModel.Period;

                TryValidateModel(reportViewModel);

                return View("Summary", reportViewModel);
            }
            catch (Exception ex)
            {
                return new BadRequestResult();
            }
        }

        [Route("Submit")]
        [Authorize(Policy = PolicyNames.CanSubmitReport)]
        public IActionResult Submit()
        {
            var report = _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);

            TryValidateModel(new ReportViewModel { Report = report });

            if (ModelState.IsValid == false)
            {
                return new RedirectResult(Url.Action("Summary", "Report"));
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

            var submittedStatus = _reportService.SubmitReport(report);

            if (submittedStatus == SubmittedStatus.Invalid)
            {
                return new RedirectResult(Url.Action("Index", "Home"));
            }

            ViewBag.CurrentPeriod = _currentPeriod;

            return View("Submitted");
        }


        //[Route("accounts/{employerAccountId}/[controller]/OrganisationName")]
        [Route("OrganisationName")]
        public IActionResult OrganisationName(string post)
        {
            var organisationVM = new OrganisationViewModel
            {
                EmployerAccount = EmployerAccount,
                Report = _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId)
            };

            if (string.IsNullOrEmpty(organisationVM.Report.OrganisationName))
                organisationVM.Report.OrganisationName = organisationVM.EmployerAccount.EmployerName;

            return View("OrganisationName", organisationVM);
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

            _reportService.SaveReport(reportViewModel.Report);

            return new RedirectResult(Url.Action("Edit", "Report"));
        }
    }
}