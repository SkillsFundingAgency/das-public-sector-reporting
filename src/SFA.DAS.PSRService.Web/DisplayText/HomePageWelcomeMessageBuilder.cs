using Microsoft.AspNetCore.Authorization;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Controllers;

namespace SFA.DAS.PSRService.Web.DisplayText;

public class HomePageWelcomeMessageBuilder(
    HomeController homeController,
    IAuthorizationService authorizationService)
{
    public PeriodSetHomepageWelcomeMessageBuilder ForPeriod(Period currentPeriod)
    {
        return new PeriodSetHomepageWelcomeMessageBuilder(
            homeController,
            authorizationService,
            currentPeriod);
    }
}