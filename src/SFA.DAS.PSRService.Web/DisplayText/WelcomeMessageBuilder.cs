using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.DisplayText
{
    public class WelcomeMessageBuilder
    {
        public static WelcomeMessageBuilder BuildWelcomeMesssage()
        {
            return new WelcomeMessageBuilder();
        }

        public UserAccessLevelWeclomeMessageBuilder ForPeriod(Period period)
        {
            return new UserAccessLevelWeclomeMessageBuilder(period);
        }
    }
}