using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.Services
{
    public interface IPeriodService
    {
        Period GetCurrentPeriod();
        bool ReportIsForCurrentPeriod(Report report);
    }
}