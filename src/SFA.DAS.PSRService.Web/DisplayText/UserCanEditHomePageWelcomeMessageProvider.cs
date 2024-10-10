using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.DisplayText;

public class UserCanEditHomePageWelcomeMessageProvider(Period period) : IReportStatusHomePageMessageBuilder
{
    public string AndReportIsInProgress()
    {
        return $"You can edit the report for the year {period.FullString} or review previously submitted reports.";
    }

    public string AndReportDoesNotExist()
    {
        return $"You can create a new report for the year {period.FullString} or review previously submitted reports.";
    }

    public string AndReportIsAlreadySubmitted()
    {
        return $"The report for the year {period.FullString} is already submitted, you can review previously submitted reports.";
    }
}