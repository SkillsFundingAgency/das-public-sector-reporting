using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Services;

public interface IEmployerAccountService
{
    EmployerIdentifier GetCurrentEmployerAccountId(HttpContext routeData);
    Task<Claim> GetClaim(string userId);
}

public class EmployerAccountService(ILogger<EmployerAccountService> logger, IAccountApiClient accountApiClient) : IEmployerAccountService
{
    private readonly ILogger<EmployerAccountService> _logger = logger;

    private async Task<IEnumerable<EmployerIdentifier>> GetEmployerIdentifiersAsync(string userId)
    {
        var accounts = await accountApiClient.GetUserAccounts(userId);

        return accounts.Select(acc => new EmployerIdentifier { AccountId = acc.HashedAccountId, EmployerName = acc.DasAccountName });
    }

    public EmployerIdentifier GetCurrentEmployerAccountId(HttpContext context)
    {
        return (EmployerIdentifier)context.Items[ContextItemKeys.EmployerIdentifier];
    }

    private async Task<string> GetUserRole(EmployerIdentifier employerAccount, string userId)
    {
        var accounts = await accountApiClient.GetAccountUsers(employerAccount.AccountId);

        if (accounts == null || accounts.Count == 0)
        {
            return null;
        }
        
        var teamMember = accounts.FirstOrDefault(c => String.Equals(c.UserRef, userId, StringComparison.CurrentCultureIgnoreCase));
        
        return teamMember?.Role;
    }

    public async Task<IEnumerable<EmployerIdentifier>> GetUserRoles(IEnumerable<EmployerIdentifier> values, string userId)
    {
        var employerIdentifiers = values.ToList();

        var identifiersToRemove = new List<EmployerIdentifier>();

        foreach (var employerIdentifier in values)
        {
            var result = await GetUserRole(employerIdentifier, userId);

            if (result != null)
            {
                employerIdentifier.Role = result;
            }
            else
            {
                identifiersToRemove.Add(employerIdentifier);
            }

        }

        return employerIdentifiers.Except(identifiersToRemove);
    }

    public async Task<Claim> GetClaim(string userId)
    {
        var accounts = await GetEmployerIdentifiersAsync(userId);

        accounts = await GetUserRoles(accounts.ToList(), userId);

        var accountsAsJson = JsonConvert.SerializeObject(accounts.ToDictionary(k => k.AccountId));
        var associatedAccountsClaim = new Claim(EmployerPsrsClaims.AccountsClaimsTypeIdentifier, accountsAsJson,
            JsonClaimValueTypes.Json);
        return associatedAccountsClaim;
    }
}