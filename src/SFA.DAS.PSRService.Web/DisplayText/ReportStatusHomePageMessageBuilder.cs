namespace SFA.DAS.PSRService.Web.DisplayText
{
    public interface ReportStatusHomePageMessageBuilder
    {
        string AndReportDoesNotExist();
        string AndReportIsAlreadySubmitted();
        string AndReportIsInProgress();
    }
}