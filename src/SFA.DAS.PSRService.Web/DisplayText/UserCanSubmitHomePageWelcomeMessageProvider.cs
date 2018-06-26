using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.DisplayText
{
    public class UserCanSubmitHomePageWelcomeMessageProvider : ReportStatusHomePageMessageBuilder
    {
        private readonly Period _period;

        public UserCanSubmitHomePageWelcomeMessageProvider(Period period)
        {
            _period = period;
        }

        public string AndReportIsInProgress()
        {
            return
                $"You can submit a new report for the year {_period.FullString} or review previously submitted reports.";
        }

        public string AndReportDoesNotExist()
        {
            return
                $"You can submit a new report for the year {_period.FullString} or review previously submitted reports.";
        }

        public string AndReportIsAlreadySubmitted()
        {
            return
                $"The report for the year {_period.FullString} is already submitted, you can review previously submitted reports.";
        }
    }
}