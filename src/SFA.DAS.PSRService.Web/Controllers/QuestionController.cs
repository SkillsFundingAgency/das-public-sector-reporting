using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Libuv.Internal.Networking;
using SFA.DAS.PSRService.Domain.Entities;
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
        private readonly IPeriodService _periodService;

        public QuestionController(IReportService reportService, IEmployerAccountService employerAccountService, IWebConfiguration webConfiguration, IPeriodService periodService) 
            : base(webConfiguration, employerAccountService)
        {
            _reportService = reportService;
            _periodService = periodService;
        }

        [Route("accounts/{employerAccountId}/[controller]/{id}")]
        public IActionResult Index(string id)
        {
            var sectionViewModel = new SectionViewModel();

            sectionViewModel.CurrentPeriod = _periodService.GetCurrentPeriod();
            sectionViewModel.Report = _reportService.GetReport(sectionViewModel.CurrentPeriod.PeriodString, EmployerAccount.AccountId);
            

            if (sectionViewModel.Report == null || (sectionViewModel.Report.IsSubmitAllowed == false && _periodService.IsSubmissionsOpen()))
                return new RedirectResult(Url.Action("Index", "Home"));

            try
            {
                sectionViewModel.CurrentSection = sectionViewModel.Report.GetQuestionSection(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to get the Current Section, see inner Exception for more details", ex);
            }

            if (sectionViewModel.CurrentSection?.Questions != null && sectionViewModel.CurrentSection.Questions.Any())
                sectionViewModel.Questions = sectionViewModel.CurrentSection.Questions.Select(s => new QuestionViewModel() {Answer = s.Answer, Id = s.Id, Optional = s.Optional, Type = s.Type}).ToList();


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

            if (Section.Report == null || Section.Report.IsSubmitAllowed == false)
                return new RedirectResult(Url.Action("Index", "Home"));

            Section.CurrentSection = Section.Report.GetQuestionSection(Section.CurrentSection.Id);

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
          
                _reportService.SaveReport(Section.Report);
                return new RedirectResult(Url.Action("Edit", "Report"));
            }
            else
            {
                Section.CurrentPeriod = Section.Report.Period;


                return View("Index", Section);
            }
            
          
        }

    }
}