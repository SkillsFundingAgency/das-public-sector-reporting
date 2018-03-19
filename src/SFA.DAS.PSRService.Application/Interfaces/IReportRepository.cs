using System;
using System.Collections.Generic;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.Interfaces
{
    public interface IReportRepository
    {       
        ReportDto Get(string period,long employerId);

        IList<ReportDto> GetSubmitted(long employerId);
        ReportDto Create(long employerId, string period);


    }
}