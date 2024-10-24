namespace SFA.DAS.PSRService.Web.Services;

public interface IDateTimeService
{
    DateTime UtcNow { get; }
}

public class SystemDateTimeService : IDateTimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
}