﻿using System.Collections.Generic;
using SFA.DAS.PSRService.Application.Domain;

namespace SFA.DAS.PSRService.Application.Interfaces
{
    public interface IReportRepository
    {       
        ReportDto Get(string period,string employerId);
        IEnumerable<ReportDto> GetSubmitted(string employerId);
        void Create(ReportDto reportDto);
        void Update(ReportDto reportDto);
    }
}