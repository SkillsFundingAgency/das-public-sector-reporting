using System;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.Interfaces
{
    public interface IReportRepository
    {       
        string Get(Guid reportId);
    }
}