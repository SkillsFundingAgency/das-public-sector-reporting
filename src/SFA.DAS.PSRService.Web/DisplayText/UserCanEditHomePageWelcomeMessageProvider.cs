using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.DisplayText
{
    public class UserCanEditHomePageWelcomeMessageProvider : ReportStatusHomePageMessageBuilder
    {
        private readonly Period _period;

        public UserCanEditHomePageWelcomeMessageProvider(Period period)
        {
            _period = period;
        }

        public string AndReportIsInProgress()
        {
            return
                $"You can edit the report for the year {_period.FullString} or review previously submitted reports.";
        }

        public string AndReportDoesNotExist()
        {
            return
                $"You can create a new report for the year {_period.FullString} or review previously submitted reports.";
        }

        public string AndReportIsAlreadySubmitted()
        {
            return
                $"The report for the year {_period.FullString} is already submitted, you can review previously submitted reports.";
        }
    }
}