using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Libuv.Internal.Networking;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Services;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.Controllers
{
    [Authorize]
    public class QuestionController : BaseController
    {
        private readonly IReportService _reportService;

        public QuestionController(IReportService reportService, IEmployerAccountService employerAccountService, IWebConfiguration webConfiguration) 
            : base(webConfiguration, employerAccountService)
        {
            _reportService = reportService;
        }

        [Route("accounts/{employerAccountId}/[controller]/{id}")]
        public IActionResult Index(string id)
        {
            var sectionViewModel = new SectionViewModel();

            sectionViewModel.Report = _reportService.GetReport(_reportService.GetCurrentReportPeriod(), EmployerAccount.AccountId);
            sectionViewModel.CurrentPeriod = _reportService.GetPeriod(sectionViewModel.Report.ReportingPeriod);

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



        [Route("accounts/{employerAccountId}/[controller]/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Submit(SectionViewModel Section)
        {
            Section.Report = _reportService.GetReport(Section.Report.ReportingPeriod, EmployerAccount.AccountId);
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

                    if (sectionQuestion.Type == QuestionType.Number)
                    {
                        sectionQuestion.Answer = sectionQuestion.Answer?.Replace(",", "");
                    }
                }
          

                _reportService.SaveQuestionSection(Section.CurrentSection, Section.Report);
                return new RedirectResult(Url.Action("Edit", "Report"));
            }
            else
            {
                Section.CurrentPeriod = _reportService.GetPeriod(Section.Report.ReportingPeriod);


                return View("Index", Section);
            }
            
          
        }

    }
}