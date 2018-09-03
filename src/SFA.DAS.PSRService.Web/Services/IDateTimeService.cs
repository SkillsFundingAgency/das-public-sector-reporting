using System;

namespace SFA.DAS.PSRService.Web.Services
{
    public interface IDateTimeService
    {
        DateTime UtcNow { get; }
    }
}