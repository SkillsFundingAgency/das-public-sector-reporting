namespace SFA.DAS.PSRService.Web.Configuration.Authorization;

public static class PolicyNames
{
    public static string HasEmployerAccount => nameof(HasEmployerAccount);
    public const string CanEditReport = nameof(CanEditReport);
    public const string CanSubmitReport = nameof(CanSubmitReport);
}