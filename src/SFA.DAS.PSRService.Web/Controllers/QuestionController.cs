using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.Services;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.Controllers
{
   // [Authorize]
    public class QuestionController : Controller
    {
        private readonly IReportService _reportService;
        private int employeeId = 12345;
        private string period = "1718";

        public QuestionController(IReportService reportService)
        {
            _reportService = reportService;
        }
        public IActionResult Index(string sectionId)
        {

            var questionViewModel = new QuestionViewModel();

            questionViewModel.Report = _reportService.GetReport(period, employeeId);

            if (questionViewModel.Report == null || _reportService.IsSubmitValid(questionViewModel.Report) == false)
                return new RedirectResult(Url.Action("Index", "Home"));

            try
            {
                questionViewModel.CurrentSection =
                    _reportService.GetQuestionSection(sectionId, questionViewModel.Report);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to get the Current Section, see inner Exception for more details", ex);
            }
            
            if (questionViewModel.CurrentSection == null)
                return new BadRequestResult();


            return View("Index", questionViewModel);
        }
    }
}