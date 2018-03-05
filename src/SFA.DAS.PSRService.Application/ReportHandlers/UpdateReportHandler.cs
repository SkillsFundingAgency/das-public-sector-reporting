using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.PSRService.Api.Types.Models;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class UpdateReportHandler : IRequestHandler<UpdateReportRequest>
    {
        public Task Handle(UpdateReportRequest message, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}