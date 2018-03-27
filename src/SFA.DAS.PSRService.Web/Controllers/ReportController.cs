using System;
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
        //private string employerId = "ABCDE";

        private string EmployerId
        {
            get
            {
                return _employerAccountService.GetCurrentEmployerAccountId(HttpContext);
            }
        }

        public ReportController(ILogger<ReportController> logger, IReportService reportService, IEmployerAccountService employerAccountService)
        {
            _logger = logger;
            _reportService = reportService;
            _employerAccountService = employerAccountService;
        }

    
    
        public IActionResult Edit(string period)
        {

            var reportViewModel = new ReportViewModel();
            
            reportViewModel.Report = _reportService.GetReport(_reportService.GetCurrentReportPeriod(), EmployerId);

            if (_reportService.IsSubmitValid(reportViewModel.Report) == false)
                return new RedirectResult(Url.Action("Index", "Home"));

            return View("Edit", reportViewModel);
        }


        [HttpGet]
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
                var report = _reportService.CreateReport(EmployerId);
                return new RedirectResult(Url.Action("Edit", "Report"));
            }
            catch (Exception ex)
            {
            
                return new BadRequestResult();
            }
          
        }

        public IActionResult List()
        {
            //need to get employee id, this needs to be moves somewhere

            var reportListViewmodel = new ReportListViewModel();

            reportListViewmodel.SubmittedReports = _reportService.GetSubmittedReports(EmployerId);

            return View("List", reportListViewmodel);
        }

        [Route("Summary/{period}")]
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
                 report.Report = _reportService.GetReport(period, EmployerId);

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

        public IActionResult Submit(string period)
        {
            var submitted = new Submitted();

            var submittedStatus = _reportService.SubmitReport(period, EmployerId, submitted);

            if (submittedStatus == SubmittedStatus.Invalid)
            {
                return new RedirectResult(Url.Action("Index","Home"));
            }

            return View("Submitted");
        }
    }
}