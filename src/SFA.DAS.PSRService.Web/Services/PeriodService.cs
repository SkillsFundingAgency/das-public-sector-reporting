using System;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.Services
{
    public class PeriodService : IPeriodService
    {
        private readonly IDateTimeService _dateTimeService;

        public PeriodService(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
        }

        public Period GetCurrentPeriod()
        {
            return Period.FromInstantInPeriod(_dateTimeService.UtcNow);
        }

        public bool PeriodIsCurrent(Period comparisonPeriod)
        {
            return comparisonPeriod.Equals(GetCurrentPeriod());
        }
    }
}
