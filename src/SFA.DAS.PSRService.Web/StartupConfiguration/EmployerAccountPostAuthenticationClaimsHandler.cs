using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Newtonsoft.Json;
using SFA.DAS.GovUK.Auth.Services;
using SFA.DAS.PSRService.Application.EmployerUserAccounts;
using SFA.DAS.PSRService.Web.Configuration;

namespace SFA.DAS.PSRService.Web.StartupConfiguration
{
    public class EmployerAccountPostAuthenticationClaimsHandler : ICustomClaims
    {
        private readonly IEmployerUserAccountsService _employerUserAccountsService;

        public EmployerAccountPostAuthenticationClaimsHandler(IEmployerUserAccountsService employerUserAccountsService)
        {
            _employerUserAccountsService = employerUserAccountsService;
        }
        public async Task<IEnumerable<Claim>> GetClaims(TokenValidatedContext tokenValidatedContext)
        {
            var userId = tokenValidatedContext.Principal.Claims
                .First(c => c.Type.Equals(ClaimTypes.NameIdentifier))
                .Value;
            var email = tokenValidatedContext.Principal.Claims
                .First(c => c.Type.Equals(ClaimTypes.Email))
                .Value;
            
            var accounts = await _employerUserAccountsService.GetEmployerUserAccounts(email, userId);
            var accountsAsJson = JsonConvert.SerializeObject(accounts.UserAccounts.ToDictionary(k => k.AccountId));
            var associatedAccountsClaim = new Claim(EmployerPsrsClaims.AccountsClaimsTypeIdentifier, accountsAsJson,
                JsonClaimValueTypes.Json);
            var returnList = new List<Claim>
            {
                associatedAccountsClaim,
                new Claim(EmployerPsrsClaims.IdamsUserIdClaimTypeIdentifier, accounts.EmployerUserId),
                new Claim(EmployerPsrsClaims.EmailClaimsTypeIdentifier, email),
                new Claim(EmployerPsrsClaims.NameClaimsTypeIdentifier, $"{accounts.FirstName} {accounts.LastName}"),
            };

            if (accounts.IsSuspended)
            {
                returnList.Add(new Claim(ClaimTypes.AuthorizationDecision, "Suspended"));
            }
            
            return returnList;
        }
    }
}