using Microsoft.AspNetCore.Authorization;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Configuration.Authorization;

namespace SFA.DAS.PSRService.Web.DisplayText;

public class PeriodSetHomepageWelcomeMessageBuilder(
    HomeController homeController,
    IAuthorizationService authorizationService,
    Period currentPeriod)
{
    public async Task<string> AndReport(Report report)
    {
        var firstStep = HomePageWelcomeMessageProvider
            .GetMessage()
            .ForPeriod(currentPeriod);

        var secondStep = await SetSecondStepBasedOnUserAccessLevel(firstStep);

        return BuildMessageBasedOnReportStatus(secondStep, report);
    }

    private static string BuildMessageBasedOnReportStatus(ReportStatusHomePageMessageBuilder secondStep, Report report)
    {
        if (report == null)
        {
            return secondStep.AndReportDoesNotExist();
        }

        if (report.Submitted)
        {
            return secondStep.AndReportIsAlreadySubmitted();
        }

        return secondStep.AndReportIsInProgress();
    }

    private async Task<ReportStatusHomePageMessageBuilder> SetSecondStepBasedOnUserAccessLevel(UserAccessLevelHomePageWelcomeMessageProvider firstStep)
    {
        if (await UserIsAuthorizedForReportSubmission())
        {
            return firstStep.WhereUserCanSubmit();
        }

        if (await UserIsAuthorizedForReportEdit())
        {
            return firstStep.WhereUserCanEdit();
        }

        return firstStep.WhereUserCanOnlyView();
    }

    private async Task<bool> UserIsAuthorizedForReportEdit()
    {
        var result = await authorizationService.AuthorizeAsync(homeController.User, homeController.ControllerContext, PolicyNames.CanEditReport);
        return result.Succeeded;
    }

    private async Task<bool> UserIsAuthorizedForReportSubmission()
    {
        var result = await authorizationService.AuthorizeAsync(homeController.User, homeController.ControllerContext, PolicyNames.CanSubmitReport);
        return result.Succeeded;
    }
}