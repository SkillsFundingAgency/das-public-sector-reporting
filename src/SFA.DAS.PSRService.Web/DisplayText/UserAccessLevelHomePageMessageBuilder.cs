using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.DisplayText
{
    public class UserAccessLevelHomePageMessageBuilder
    {
        private readonly Period _period;

        public UserAccessLevelHomePageMessageBuilder(Period period)
        {
            _period = period;
        }

        public UserCanOnlyViewHomePageMessageBuilder WhereUserCanOnlyView()
        {
            return new UserCanOnlyViewHomePageMessageBuilder(_period);
        }

        public UserCanEditHomePageMessageBuilder WhereUserCanEdit()
        {
            return new UserCanEditHomePageMessageBuilder(_period);
        }

        public UserCanSubmitHomePageMessageBuilder WhereUserCanSubmit()
        {
            return new UserCanSubmitHomePageMessageBuilder(_period);
        }
    }
}