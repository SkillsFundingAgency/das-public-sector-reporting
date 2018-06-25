using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.DisplayText
{
    public class HomePageMessageBuilder
    {
        public static HomePageMessageBuilder BuildMesssage()
        {
            return new HomePageMessageBuilder();
        }

        public UserAccessLevelHomePageMessageBuilder ForPeriod(Period period)
        {
            return new UserAccessLevelHomePageMessageBuilder(period);
        }
    }
}