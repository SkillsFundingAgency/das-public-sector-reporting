namespace SFA.DAS.PSRService.Web.Services
{
    public class SystemDateTimeService : IDateTimeService
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}