﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Models.Home;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IReportService _reportService;
        private const long EmployerId = 123; // TODO: get this from context

        public HomeController(IReportService reportService)
        {
            _reportService = reportService;
        }

        public IActionResult Index()
        {
            var model = new IndexViewModel();
            var period = _reportService.GetCurrentReportPeriod();
            var report = _reportService.GetReport(period, EmployerId);
            model.PeriodName = _reportService.GetCurrentReportPeriodName(period);
            model.CanCreateReport = report == null;
            model.CanEditReport = report != null && !report.Submitted;

            return View(model);
        }

        public IActionResult Submit(string action)
        {
            if (action == "create")
                return new RedirectResult(Url.Action("Create", "Report"));

            if (action == "list")
                return new RedirectResult(Url.Action("List", "Report"));

            return new BadRequestResult();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
