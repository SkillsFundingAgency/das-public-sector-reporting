using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.GovUK.Auth.Employer;
using SFA.DAS.PSRService.Application.EmployerUserAccounts.Requests;
using SFA.DAS.PSRService.Application.EmployerUserAccounts.Responses;
using SFA.DAS.PSRService.Application.OuterApi;

namespace SFA.DAS.PSRService.Application.Services;

public class UserAccountService(IOuterApiClient outerApiClient) : IGovAuthEmployerAccountService
{
    async Task<GovUK.Auth.Employer.EmployerUserAccounts> IGovAuthEmployerAccountService.GetUserAccounts(string userId, string email)
    {
        var response = await outerApiClient.Get<GetUserAccountsResponse>(new GetUserAccountsRequest(userId, email));
        var actual = response.Body;

        return new GovUK.Auth.Employer.EmployerUserAccounts
        {
            EmployerAccounts = actual.UserAccounts != null? actual.UserAccounts.Select(c => new EmployerUserAccountItem
            {
                Role = c.Role,
                AccountId = c.AccountId,
                EmployerName = c.EmployerName,
            }).ToList() : [],
            FirstName = actual.FirstName,
            IsSuspended = actual.IsSuspended,
            LastName = actual.LastName,
            EmployerUserId = actual.EmployerUserId,
        };
    }
}