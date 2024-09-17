using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.Controllers;

[Authorize(Policy = PolicyNames.CanEditReport)]
public class QuestionController(
    IReportService reportService,
    IEmployerAccountService employerAccountService,
    IWebConfiguration webConfiguration,
    IPeriodService periodService,
    IUserService userService) : BaseController(webConfiguration, employerAccountService)
{
    [Route("accounts/{hashedEmployerAccountId}/[controller]/{id}")]
    public async Task<IActionResult> Index(string id)
    {
        var currentPeriod = periodService.GetCurrentPeriod();
        var report = await reportService.GetReport(currentPeriod.PeriodString, EmployerAccount.AccountId);

        if (!reportService.CanBeEdited(report))
        {
            return new RedirectResult(Url.Action("Index", "Home"));
        }

        var currentSection = report.GetQuestionSection(id);

        if (currentSection == null)
        {
            return new NotFoundResult();
        }

        var sectionViewModel = new SectionViewModel
        {
            CurrentPeriod = currentPeriod,
            Report = report,
            CurrentSection = currentSection
        };

        if (sectionViewModel.CurrentSection.Questions != null)
        {
            sectionViewModel.Questions = currentSection.Questions.Select(s => new QuestionViewModel
            {
                Answer = s.Answer,
                Id = s.Id,
                Optional = s.Optional,
                Type = s.Type
            }).ToList();
        }

        return View("Index", sectionViewModel);
    }

    [Route("accounts/{hashedEmployerAccountId}/[controller]/{id}")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(SectionModel section)
    {
        var report = await reportService.GetReport(section.ReportingPeriod, EmployerAccount.AccountId);

        if (!reportService.CanBeEdited(report))
        {
            return new RedirectResult(Url.Action("Index", "Home"));
        }

        var currentSection = report.GetQuestionSection(section.Id);

        if (currentSection == null || section.Questions == null)
        {
            return new BadRequestResult();
        }

        if (ModelState.IsValid)
        {
            foreach (var question in currentSection.Questions)
            {
                var answeredQuestion = section.Questions.SingleOrDefault(w => w.Id == question.Id);
                if (answeredQuestion == null)
                {
                    continue;
                }

                if (question.Type == QuestionType.Number && !string.IsNullOrEmpty(answeredQuestion.Answer))
                {
                    question.Answer = int.Parse(answeredQuestion.Answer.Trim(), NumberStyles.AllowThousands).ToString();
                }
                else
                {
                    question.Answer = answeredQuestion.Answer;
                }
            }

            var userModel = userService.GetUserModel(User);
            await reportService.SaveReport(report, userModel, null);

            return new RedirectResult(Url.Action("Edit", "Report"));
        }

        var viewModel = new SectionViewModel
        {
            CurrentPeriod = report.Period,
            CurrentSection = currentSection,
            Report = report,
            Questions = section.Questions
        };

        return View("Index", viewModel);
    }
}