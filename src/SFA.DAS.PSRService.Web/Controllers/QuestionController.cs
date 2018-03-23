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
    [Authorize]
    [Route("Question")]
    public class QuestionController : Controller
    {
        private readonly IReportService _reportService;
       
        private int employeeId;
      

        public QuestionController(IReportService reportService)
        {
            _reportService = reportService;
            employeeId = 12345;
        }
        [Route("/[controller]/{id}")]
        public IActionResult Index(string id)
        {

            var sectionViewModel = new SectionViewModel();

            sectionViewModel.Report = _reportService.GetReport(_reportService.GetCurrentReportPeriod(), employeeId);

            if (sectionViewModel.Report == null || _reportService.IsSubmitValid(sectionViewModel.Report) == false)
                return new RedirectResult(Url.Action("Index", "Home"));

            try
            {
                sectionViewModel.CurrentSection =
                    _reportService.GetQuestionSection(id, sectionViewModel.Report);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to get the Current Section, see inner Exception for more details", ex);
            }
            if(sectionViewModel.CurrentSection?.Questions != null && sectionViewModel.CurrentSection.Questions.Any())
            sectionViewModel.Questions = sectionViewModel.CurrentSection.Questions.Select(s => new QuestionViewModel(){Answer = s.Answer, Id = s.Id, Optional = s.Optional,Type = s.Type}).ToList();


            if (sectionViewModel.CurrentSection == null)
                return new BadRequestResult();


            return View("Index", sectionViewModel);
        }

        [Route("/[controller]/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Submit(SectionViewModel Section)
        {
            Section.Report = _reportService.GetReport(Section.Report.ReportingPeriod, Section.Report.EmployerId);
            Section.CurrentSection = _reportService.GetQuestionSection(Section.CurrentSection.Id, Section.Report);

            if (Section.Report == null || _reportService.IsSubmitValid(Section.Report) == false)
                return new RedirectResult(Url.Action("Index", "Home"));

            if (Section.CurrentSection == null)
                return new BadRequestResult();

            if (ModelState.IsValid)
            {

                foreach (var sectionQuestion in Section.CurrentSection.Questions)
                {
                    sectionQuestion.Answer = Section.Questions.Single(w => w.Id == sectionQuestion.Id).Answer;
                }
          

                _reportService.SaveQuestionSection(Section.CurrentSection, Section.Report);
                return new RedirectResult(Url.Action("Edit", "Report"));
            }
            else
            {
                return View("Index", Section);
            }
            
          
        }

    }
}