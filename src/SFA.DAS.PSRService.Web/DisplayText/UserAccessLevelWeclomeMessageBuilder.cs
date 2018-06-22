using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.DisplayText
{
    public class UserAccessLevelWeclomeMessageBuilder
    {
        private readonly Period _period;

        public UserAccessLevelWeclomeMessageBuilder(Period period)
        {
            _period = period;
        }

        public UserCanOnlyViewWelcomeMessageBuilder WhereUserCanOnlyView()
        {
            return new UserCanOnlyViewWelcomeMessageBuilder(_period);
        }

        public UserCanEditWelcomeMessageBuilder WhereUserCanEdit()
        {
            return new UserCanEditWelcomeMessageBuilder(_period);
        }

        public UserCanSubmitWelcomeMessageBuilder WhereUserCanSubmit()
        {
            return new UserCanSubmitWelcomeMessageBuilder(_period);
        }
    }
}