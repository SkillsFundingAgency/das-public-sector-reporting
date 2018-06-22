namespace SFA.DAS.PSRService.Web.DisplayText
{
    public interface ReportStatusWelcomeMessageBuilder
    {
        string AndReportDoesNotExist();
        string AndReportIsAlreadySubmitted();
        string AndReportIsInProgress();
    }
}