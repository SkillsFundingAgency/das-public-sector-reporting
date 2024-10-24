using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.DisplayText;

public class UserCanOnlyViewHomePageWelcomeMessageProvider(Period period) : IReportStatusHomePageMessageBuilder
{
    public string AndReportIsInProgress()
    {
        return $"You can view the report for the year {period.FullString} or review previously submitted reports.";
    }

    public string AndReportDoesNotExist()
    {
        return $"The report for the year {period.FullString} has not been created, you can review previously submitted reports.";
    }

    public string AndReportIsAlreadySubmitted()
    {
        return $"The report for the year {period.FullString} is already submitted, you can review previously submitted reports.";
    }
}