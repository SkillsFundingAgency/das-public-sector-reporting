using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.DisplayText;

public class HomePageWelcomeMessageProvider
{
    public static HomePageWelcomeMessageProvider GetMessage()
    {
        return new HomePageWelcomeMessageProvider();
    }

    public UserAccessLevelHomePageWelcomeMessageProvider ForPeriod(Period period)
    {
        return new UserAccessLevelHomePageWelcomeMessageProvider(period);
    }
}