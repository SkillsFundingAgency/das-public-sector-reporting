using SFA.DAS.PSRService.Api.Types.Models;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;
using Report = SFA.DAS.PSRService.Api.Types.Models.Report;

namespace SFA.DAS.PSRService.Application.ContactHandlers
{
    using AutoMapper;
    using Domain;
    using MediatR;
    using Interfaces;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateReportHandler : IRequestHandler<CreateReportRequest, Report>
    {
        public Task<Report> Handle(CreateReportRequest request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}