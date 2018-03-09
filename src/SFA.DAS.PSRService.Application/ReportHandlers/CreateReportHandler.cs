using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class CreateReportHandler : IRequestHandler<CreateReportRequest, Report>
    {
        public Task<Report> Handle(CreateReportRequest request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}