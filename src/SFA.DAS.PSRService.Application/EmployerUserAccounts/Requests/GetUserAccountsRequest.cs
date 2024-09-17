using System.Net;
using SFA.DAS.PSRService.Application.OuterApi;

namespace SFA.DAS.PSRService.Application.EmployerUserAccounts.Requests;

public class GetUserAccountsRequest(string userId, string email) : IGetApiRequest
{
    private readonly string _email = WebUtility.UrlEncode(email);

    public string GetUrl => $"accountusers/{userId}/accounts?email={_email}";
}