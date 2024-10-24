using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.DisplayText;

public class UserAccessLevelHomePageWelcomeMessageProvider(Period period)
{
    public UserCanOnlyViewHomePageWelcomeMessageProvider WhereUserCanOnlyView()
    {
        return new UserCanOnlyViewHomePageWelcomeMessageProvider(period);
    }

    public UserCanEditHomePageWelcomeMessageProvider WhereUserCanEdit()
    {
        return new UserCanEditHomePageWelcomeMessageProvider(period);
    }

    public UserCanSubmitHomePageWelcomeMessageProvider WhereUserCanSubmit()
    {
        return new UserCanSubmitHomePageWelcomeMessageProvider(period);
    }
}