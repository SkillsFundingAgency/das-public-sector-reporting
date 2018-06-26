using System;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Configuration.Authorization;

namespace SFA.DAS.PSRService.Web.DisplayText
{
    public class PeriodSetHomepageWelcomeMessageBuilder
    {
        private readonly HomeController _homeController;
        private readonly IAuthorizationService _authorizationService;
        private readonly Period _currentPeriod;

        public PeriodSetHomepageWelcomeMessageBuilder(
            HomeController homeController,
            IAuthorizationService authorizationService,
            Period currentPeriod)
        {
            _homeController = homeController ?? throw new ArgumentNullException(nameof(homeController));
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
            _currentPeriod = currentPeriod ?? throw new ArgumentNullException(nameof(currentPeriod));
        }

        public string AndReport(Report report)
        {
            var firstStep =
                HomePageWelcomeMessageProvider
                    .GetMesssage()
                    .ForPeriod(_currentPeriod);

            var secondStep = 
                SetSecondStepBasedOnUserAccessLevel(firstStep);

            return
                BuildMessageBasedOnReportStatus(
                    secondStep,
                    report);
        }

        private string BuildMessageBasedOnReportStatus(
            ReportStatusHomePageMessageBuilder secondStep,
            Report report)
        {
            if (report == null)
                return
                    secondStep
                        .AndReportDoesNotExist();

            if (report.Submitted)
                return
                    secondStep
                        .AndReportIsAlreadySubmitted();

            return
                secondStep
                    .AndReportIsInProgress();
        }

        private ReportStatusHomePageMessageBuilder SetSecondStepBasedOnUserAccessLevel(UserAccessLevelHomePageWelcomeMessageProvider firstStep)
        {
            if (UserIsAuthorizedForReportSubmission())
                return
                    firstStep
                        .WhereUserCanSubmit();

            if (UserIsAuthorizedForReportEdit())
                return
                    firstStep
                        .WhereUserCanEdit();

            return
                firstStep
                    .WhereUserCanOnlyView();
        }

        private bool UserIsAuthorizedForReportEdit()
        {
            return
                _authorizationService
                    .AuthorizeAsync(
                        _homeController.User,
                        _homeController.ControllerContext,
                        PolicyNames.CanEditReport)
                    .Result
                    .Succeeded;
        }

        private bool UserIsAuthorizedForReportSubmission()
        {
            return
                _authorizationService
                    .AuthorizeAsync(
                        _homeController.User,
                        _homeController.ControllerContext,
                        PolicyNames.CanSubmitReport)
                    .Result
                    .Succeeded;
        }
    }
}