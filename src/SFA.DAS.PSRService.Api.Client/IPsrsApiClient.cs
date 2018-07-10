using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.PSRService.Api.Types;

namespace SFA.DAS.PSRService.Api.Client
{
    public interface IPsrsApiClient
    {
        Task<PsrsStatisticsUpdatedMessage> GetStatistics();
        Task<ICollection<ReportSubmittedMessage>> GetSubmittedReports();
    }
}
