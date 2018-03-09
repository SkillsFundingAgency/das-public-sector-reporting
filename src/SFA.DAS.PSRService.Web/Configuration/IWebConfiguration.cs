namespace SFA.DAS.PSRService.Web.Configuration
{
    public interface IWebConfiguration
    {
        AuthSettings Authentication { get; set; }
        string SqlConnectionString { get; set; }
    }
}