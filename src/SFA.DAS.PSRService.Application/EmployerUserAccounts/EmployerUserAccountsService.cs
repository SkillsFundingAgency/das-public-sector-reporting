using System.Net;
using System.Threading.Tasks;
using SFA.DAS.PSRService.Application.EmployerUserAccounts.Requests;
using SFA.DAS.PSRService.Application.EmployerUserAccounts.Responses;
using SFA.DAS.PSRService.Application.OuterApi;

namespace SFA.DAS.PSRService.Application.EmployerUserAccounts
{
    public interface IEmployerUserAccountsService
    {
        Task<EmployerUserAccounts> GetEmployerUserAccounts(string email, string userId);
    }
    public class EmployerUserAccountsService : IEmployerUserAccountsService
    {
        private readonly IOuterApiClient _apiClient;

        public EmployerUserAccountsService(IOuterApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<EmployerUserAccounts> GetEmployerUserAccounts(string email, string userId)
        {
            var response = await _apiClient.Get<GetUserAccountsResponse>(new GetUserAccountsRequest(userId, email));

            switch (response.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return new GetUserAccountsResponse();
                case HttpStatusCode.InternalServerError:
                    return null;
                default:
                    return response.Body;
            }
        }
    }
}