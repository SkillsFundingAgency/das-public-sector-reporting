using System;
using System.Collections.Generic;
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

        public IActionResult EditTest()
        {
            var Questions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "0",
                    Type = QuestionType.Number,
                    Optional = false
                }
                ,new Question()
                {
                    Id = "atEnd",
                    Answer = "0",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "newThisPeriod",
                    Answer = "0",
                    Type = QuestionType.Number,
                    Optional = false
                }

            };

            var Section = new Section()
            {
                Id = "ReportNumbers",
                SubSections = new List<Section>() { new Section{
                    Id = "YourEmployees",
                    Questions = Questions,
                    Title = "",
                    SummaryText = "Number of employees who work in England"

                }},
                Questions = null,
                Title = "Report numbers in the following categories"
            };





            IList<Section> sections = new List<Section>();

            sections.Add(Section);
            sections.Add(Section);

            var report = new Report()
            {
                Sections = sections
            };

            var reportVM = new ReportViewModel()
            {
                Report = report
            };



            return View("Edit", reportVM);
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