﻿using System.Globalization;
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
[Route("accounts/{hashedEmployerAccountId}/[controller]")]
public class QuestionController(
    IReportService reportService,
    IEmployerAccountService employerAccountService,
    IWebConfiguration webConfiguration,
    IPeriodService periodService) : BaseController(webConfiguration, employerAccountService)
{
    [Route("{id}")]
    public async Task<IActionResult> Index(string id)
    {
        var currentPeriod = periodService.GetCurrentPeriod();
        var report = await reportService.GetReport(currentPeriod.PeriodString, EmployerAccount.AccountId);

        if (!reportService.CanBeEdited(report))
        {
            return RedirectToAction("Index", "Home");
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

        return View(nameof(Index), sectionViewModel);
    }

    [HttpPost]
    [Route("{id}")]
    public async Task<IActionResult> Submit(SectionModel section)
    {
        var report = await reportService.GetReport(section.ReportingPeriod, EmployerAccount.AccountId);

        if (!reportService.CanBeEdited(report))
        {
            return RedirectToAction(nameof(Index), "Home");
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

            var userModel = UserModel.From(User);
            await reportService.SaveReport(report, userModel, null);

            return RedirectToAction("Edit", "Report");
        }

        var viewModel = new SectionViewModel
        {
            CurrentPeriod = report.Period,
            CurrentSection = currentSection,
            Report = report,
            Questions = section.Questions
        };

        return View(nameof(Index), viewModel);
    }
}