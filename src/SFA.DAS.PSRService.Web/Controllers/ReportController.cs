using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.Services;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.Controllers
{
   // [Authorize]
    public class ReportController : Controller
    {
        private readonly ILogger<ReportController> _logger;
        private readonly IReportService _reportService;
        private long employerId = 123;

        public ReportController(ILogger<ReportController> logger, IReportService reportService)
        {
            _logger = logger;
            _reportService = reportService;
        }

        public IActionResult Edit(string period)
        {
            
            var report = _reportService.GetReport(period, employerId);

            if (_reportService.IsSubmitValid(report) == false)
                return new RedirectResult(Url.Action("Index", "Home"));

            return View("Edit", report);
        }

        public IActionResult Create()
        {
            try
            {
                var report = _reportService.CreateReport(employerId);
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

            reportListViewmodel.SubmittedReports = _reportService.GetSubmittedReports(employerId);

            
                return View("List", reportListViewmodel);

            
        }

        public IActionResult Summary(string period)
        {
            try
            {
                var report = _reportService.GetReport(period, employerId);
            
           

            if (report == null)
                return new RedirectResult(Url.Action("Index","Home"));

            return View("Summary",report);
            }
            catch (Exception ex)
            {

                return new BadRequestResult();
            }
        }

        public IActionResult Submit(string period)
        {
           
            var submitted = new Submitted();

            
           var submittedStatus = _reportService.SubmitReport(period,employerId,submitted);

            if (submittedStatus == SubmittedStatus.Invalid)
            {
                return new RedirectResult(Url.Action("Index","Home"));
            }

            return View("Submitted");


        }
    }
}