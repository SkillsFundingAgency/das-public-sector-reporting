using System;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.PSRService.Web.Controllers;

namespace SFA.DAS.PSRService.Web.DisplayText
{
    public class HomePageMessageProvider
    {
        private readonly HomeController homeController;
        private readonly IAuthorizationService _authorizationService;

        public HomePageMessageProvider(
            HomeController homeController, 
            IAuthorizationService _authorizationService)
        {
            this.homeController = homeController ?? throw new ArgumentNullException(nameof(homeController));
            this._authorizationService = _authorizationService ?? throw new ArgumentNullException(nameof(_authorizationService));
        }

        public HomePageWelcomeMessageBuilder GetWelcomeMessage()
        {
            return new HomePageWelcomeMessageBuilder(homeController, _authorizationService);
        }
    }
}