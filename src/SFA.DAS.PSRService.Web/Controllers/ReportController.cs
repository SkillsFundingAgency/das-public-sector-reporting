using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Models;
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
        private readonly IPeriodService _periodService;
        private readonly Period _currentPeriod;

        public ReportController(ILogger<ReportController> logger, IReportService reportService, IEmployerAccountService employerAccountService, IUserService userService, IWebConfiguration webConfiguration, IPeriodService periodService)
            : base(webConfiguration, employerAccountService)
        {
            _reportService = reportService;
            _userService = userService;
            _periodService = periodService;
            _currentPeriod = _periodService.GetCurrentPeriod();
        }

        public IActionResult Edit(string period)
        {

            var reportViewModel = new ReportViewModel();

            reportViewModel.Report = _reportService.GetReport(_currentPeriod.PeriodString, EmployerAccount.AccountId);

            if (reportViewModel.Report != null && reportViewModel.Report.IsSubmitAllowed)
            {
                return View("Edit", reportViewModel);
            }

            return new RedirectResult(Url.Action("Index", "Home"));


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
                var report = _reportService.CreateReport(EmployerAccount.AccountId);
                return new RedirectResult(Url.Action("Edit", "Report"));
            }
            catch (Exception ex)
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
                {
                    period = _currentPeriod.PeriodString;
                }

                var reportViewModel = new ReportViewModel
                {
                    Report = _reportService.GetReport(period, EmployerAccount.AccountId),
                    Period = _currentPeriod
                };

                if (reportViewModel.Report == null)
                    return new RedirectResult(Url.Action("Index", "Home"));

                reportViewModel.SubmitValid = reportViewModel.Report.IsSubmitAllowed;
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
        public IActionResult Submit(string period)
        {
            if (period == null)
                period = _currentPeriod.PeriodString;

            var report = new ReportViewModel();
            report.Report = _reportService.GetReport(period, EmployerAccount.AccountId);

            TryValidateModel(report);
            if (ModelState.IsValid == false)
            {
                return new RedirectResult(Url.Action("Summary", "Report"));
            }


            var user = _userService.GetUserModel(this.User);

            var submitted = new Submitted();

            submitted.SubmittedAt = DateTime.UtcNow;
            submitted.SubmittedEmail = user.Email;
            submitted.SubmittedName = user.DisplayName;
            submitted.SubmttedBy = user.Id.ToString();
            submitted.UniqueReference = "NotAUniqueReference";



            var submittedStatus = _reportService.SubmitReport(period, EmployerAccount.AccountId, submitted);

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