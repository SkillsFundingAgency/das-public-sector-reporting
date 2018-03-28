﻿using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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
    public class ReportController : Controller
    {
        private readonly ILogger<ReportController> _logger;
        private readonly IReportService _reportService;
        private readonly IEmployerAccountService _employerAccountService;

        private readonly IUserService _userService;
        //private string employerId = "ABCDE";

        private EmployerIdentifier EmployerAccount => _employerAccountService.GetCurrentEmployerAccountId(HttpContext);

        public ReportController(ILogger<ReportController> logger, IReportService reportService, IEmployerAccountService employerAccountService, IUserService userService)
        {
            _logger = logger;
            _reportService = reportService;
            _employerAccountService = employerAccountService;
            _userService = userService;
        }

    
    
        public IActionResult Edit(string period)
        {

            var reportViewModel = new ReportViewModel();
            
            reportViewModel.Report = _reportService.GetReport(_reportService.GetCurrentReportPeriod(), EmployerAccount.AccountId);

            if (_reportService.IsSubmitValid(reportViewModel.Report) == false)
                return new RedirectResult(Url.Action("Index", "Home"));

            return View("Edit", reportViewModel);
        }


        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
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

            return View("List", reportListViewmodel);
        }

        [Route("Summary/{period}")]
        [Route("Summary")]
        public IActionResult Summary(string period)
        {
            try
            {
                var currentPeriod = _reportService.GetCurrentReportPeriod();
                if (period == null)
                {
                    period = currentPeriod;
                }

                var report = new ReportViewModel();
                 report.Report = _reportService.GetReport(period, EmployerAccount.AccountId);

                report.CurrentPeriod = currentPeriod;
                report.SubmitValid = _reportService.IsSubmitValid(report.Report);
                

                if (report.Report == null)
                    return new RedirectResult(Url.Action("Index", "Home"));

                return View("Summary", report);
            }
            catch (Exception ex)
            {
                return new BadRequestResult();
            }
        }
        [Route("Submit")]
        public IActionResult Submit(string period)
        {

            var user = _userService.GetUserModel(this.User);

            var submitted = new Submitted();

            submitted.SubmittedAt = DateTime.UtcNow;
            submitted.SubmittedEmail = user.Email;
            submitted.SubmittedName = user.DisplayName;
            submitted.SubmttedBy = user.Id.ToString();
            submitted.UniqueReference = "NotAUniqueReference";

            if (period == null)
                period = _reportService.GetCurrentReportPeriod();

            var submittedStatus = _reportService.SubmitReport(period, EmployerAccount.AccountId, submitted);

            if (submittedStatus == SubmittedStatus.Invalid)
            {
                return new RedirectResult(Url.Action("Index","Home"));
            }

            return View("Submitted");
        }

        [Route("OrganisationName")]
        public IActionResult OrganisationName(string post)
        {
            var organisationVM = new OrganisationViewModel
            {
                EmployerAccount = EmployerAccount,
                Report = _reportService.GetReport(_reportService.GetCurrentReportPeriod(), EmployerAccount.AccountId)
            };

            if (organisationVM.Report == null)
                return new RedirectResult(Url.Action("Index", "Home"));

            return View("OrganisationName", organisationVM);
        }

        [Route("Change")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Change(OrganisationViewModel organisationVm)
        {
            var reportViewModel = new ReportViewModel
            {
                Report = _reportService.GetReport(_reportService.GetCurrentReportPeriod(), EmployerAccount.AccountId)
            };

            if (reportViewModel.Report == null)
                return new RedirectResult(Url.Action("Index", "Home"));

            reportViewModel.Report.OrganisationName = organisationVm.Report.OrganisationName;

            _reportService.SaveReport(reportViewModel.Report);

            return new RedirectResult(Url.Action("Edit", "Report"));
        }

    }
}