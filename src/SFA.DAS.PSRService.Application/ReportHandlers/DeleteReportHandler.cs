using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.PSRService.Api.Types.Models;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{

    public class DeleteReportHandler : IRequestHandler<DeleteReportRequest>
    {
        public Task Handle(DeleteReportRequest message, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}