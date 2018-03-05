using SFA.DAS.PSRService.Api.Types.Models;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Domain.Entities;
using Report = SFA.DAS.PSRService.Api.Types.Models.Report;

namespace SFA.DAS.PSRService.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Enums;
    using Domain.Exceptions;
    using Microsoft.EntityFrameworkCore;

    public class ReportRepository : IReportRepository
    {
        public Task<Report> CreateNewContact(ReportCreateDomainModel newContact)
        {
            throw new NotImplementedException();
        }

        public Task Update(UpdateReportRequest organisationUpdateViewModel)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string userName)
        {
            throw new NotImplementedException();
        }
    }
}