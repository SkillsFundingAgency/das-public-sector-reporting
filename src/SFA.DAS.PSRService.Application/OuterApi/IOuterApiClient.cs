using System.Threading.Tasks;
using SFA.DAS.PSRService.Application.OuterApi.Requests;

namespace SFA.DAS.PSRService.Application.OuterApi
{
    public interface IOuterApiClient
    {
        Task<ApiResponse<TResponse>> Get<TResponse>(IGetApiRequest request);
    }
}