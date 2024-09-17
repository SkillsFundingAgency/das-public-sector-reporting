using System.Threading.Tasks;

namespace SFA.DAS.PSRService.Application.OuterApi;

public interface IOuterApiClient
{
    Task<ApiResponse<TResponse>> Get<TResponse>(IGetApiRequest request);
}