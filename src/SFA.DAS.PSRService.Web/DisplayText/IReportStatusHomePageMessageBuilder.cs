namespace SFA.DAS.PSRService.Web.DisplayText;

public interface IReportStatusHomePageMessageBuilder
{
    string AndReportDoesNotExist();
    string AndReportIsAlreadySubmitted();
    string AndReportIsInProgress();
}