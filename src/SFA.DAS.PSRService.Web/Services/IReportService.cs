using System;
using System.Threading.Tasks;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.Services
{
    public interface IReportService
    {
        Report GetReport(Guid reportId);
    }
}