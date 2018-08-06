using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.DisplayText
{
    public class UserAccessLevelHomePageWelcomeMessageProvider
    {
        private readonly Period _period;

        public UserAccessLevelHomePageWelcomeMessageProvider(Period period)
        {
            _period = period;
        }

        public UserCanOnlyViewHomePageWelcomeMessageProvider WhereUserCanOnlyView()
        {
            return new UserCanOnlyViewHomePageWelcomeMessageProvider(_period);
        }

        public UserCanEditHomePageWelcomeMessageProvider WhereUserCanEdit()
        {
            return new UserCanEditHomePageWelcomeMessageProvider(_period);
        }

        public UserCanSubmitHomePageWelcomeMessageProvider WhereUserCanSubmit()
        {
            return new UserCanSubmitHomePageWelcomeMessageProvider(_period);
        }
    }
}