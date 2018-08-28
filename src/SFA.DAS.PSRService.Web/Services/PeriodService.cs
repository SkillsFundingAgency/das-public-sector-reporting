using System;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.Services
{
    public class PeriodService : IPeriodService
    {
        public Period GetCurrentPeriod()
        {
            return new Period(DateTime.UtcNow);
        }

        public bool ReportIsForCurrentPeriod(Report report)
        {
            throw new NotImplementedException();
        }
    }
}
