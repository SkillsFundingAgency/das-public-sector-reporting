using System.Net;
using System.Threading.Tasks;
using SFA.DAS.PSRService.Application.EmployerUserAccounts.Requests;
using SFA.DAS.PSRService.Application.EmployerUserAccounts.Responses;
using SFA.DAS.PSRService.Application.OuterApi;

namespace SFA.DAS.PSRService.Application.EmployerUserAccounts;

public interface IEmployerUserAccountsService
{
    Task<EmployerUserAccounts> GetEmployerUserAccounts(string email, string userId);
}

public class EmployerUserAccountsService(IOuterApiClient apiClient) : IEmployerUserAccountsService
{
    public async Task<EmployerUserAccounts> GetEmployerUserAccounts(string email, string userId)
    {
        var response = await apiClient.Get<GetUserAccountsResponse>(new GetUserAccountsRequest(userId, email));

        return response.StatusCode switch
        {
            HttpStatusCode.NotFound => new GetUserAccountsResponse(),
            HttpStatusCode.InternalServerError => (EmployerUserAccounts)null,
            _ => response.Body
        };
    }
}