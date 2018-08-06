using System;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Controllers;

namespace SFA.DAS.PSRService.Web.DisplayText
{
    public class HomePageWelcomeMessageBuilder
    {
        private readonly HomeController _homeController;
        private readonly IAuthorizationService _authorizationService;

        public HomePageWelcomeMessageBuilder(
            HomeController homeController, 
            IAuthorizationService authorizationService)
        {
            _homeController = homeController ?? throw new ArgumentNullException(nameof(homeController));
            this._authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(_authorizationService));
        }

        public PeriodSetHomepageWelcomeMessageBuilder ForPeriod(Period currentPeriod)
        {
            return new PeriodSetHomepageWelcomeMessageBuilder(
                _homeController,
                _authorizationService,
                currentPeriod);
        }
    }
}