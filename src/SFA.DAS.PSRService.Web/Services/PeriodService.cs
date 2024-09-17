using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.Services;

public class PeriodService(IDateTimeService dateTimeService) : IPeriodService
{
    public Period GetCurrentPeriod()
    {
        return Period.FromInstantInPeriod(dateTimeService.UtcNow);
    }

    public bool PeriodIsCurrent(Period comparisonPeriod)
    {
        return comparisonPeriod.Equals(GetCurrentPeriod());
    }
}