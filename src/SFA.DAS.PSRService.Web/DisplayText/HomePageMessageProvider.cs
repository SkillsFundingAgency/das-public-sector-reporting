using Microsoft.AspNetCore.Authorization;
using SFA.DAS.PSRService.Web.Controllers;

namespace SFA.DAS.PSRService.Web.DisplayText;

public class HomePageMessageProvider(
    HomeController homeController,
    IAuthorizationService authorizationService)
{
    public HomePageWelcomeMessageBuilder GetWelcomeMessage()
    {
        return new HomePageWelcomeMessageBuilder(homeController, authorizationService);
    }
}