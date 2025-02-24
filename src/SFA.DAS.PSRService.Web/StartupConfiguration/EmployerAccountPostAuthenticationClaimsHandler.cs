using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Newtonsoft.Json;
using SFA.DAS.GovUK.Auth.Employer;
using SFA.DAS.GovUK.Auth.Services;
using SFA.DAS.PSRService.Web.Configuration;

namespace SFA.DAS.PSRService.Web.StartupConfiguration;

public class EmployerAccountPostAuthenticationClaimsHandler(IGovAuthEmployerAccountService employerUserAccountsService) : ICustomClaims
{
    public async Task<IEnumerable<Claim>> GetClaims(TokenValidatedContext tokenValidatedContext)
    {
        var userId = tokenValidatedContext.Principal.Claims
            .First(c => c.Type.Equals(ClaimTypes.NameIdentifier))
            .Value;

        var email = tokenValidatedContext.Principal.Claims
            .First(c => c.Type.Equals(ClaimTypes.Email))
            .Value;

        var accounts = await employerUserAccountsService.GetUserAccounts(userId, email);
        var accountsAsJson = JsonConvert.SerializeObject(accounts.EmployerAccounts.ToDictionary(k => k.AccountId));

        var associatedAccountsClaim = new Claim(EmployerPsrsClaims.AccountsClaimsTypeIdentifier, accountsAsJson, JsonClaimValueTypes.Json);

        var returnList = new List<Claim>
        {
            associatedAccountsClaim,
            new(EmployerPsrsClaims.IdamsUserIdClaimTypeIdentifier, accounts.EmployerUserId),
            new(EmployerPsrsClaims.EmailClaimsTypeIdentifier, email),
            new(EmployerPsrsClaims.NameClaimsTypeIdentifier, $"{accounts.FirstName} {accounts.LastName}"),
        };

        if (accounts.IsSuspended)
        {
            returnList.Add(new Claim(ClaimTypes.AuthorizationDecision, "Suspended"));
        }

        return returnList;
    }
}