﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.PSRService.Web.Models;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using SFA.DAS.PSRService.Application.Handler.EmployerAccountHandler.GetUserAccountRole;
using SFA.DAS.PSRService.Web.Configuration;

namespace SFA.DAS.PSRService.Web.Services
{
    public class EmployerAccountService : IEmployerAccountService
    {
        private readonly ILogger<EmployerAccountService> _logger;
        private readonly IAccountApiClient _accountApiClient;

        public EmployerAccountService(ILogger<EmployerAccountService> logger, IAccountApiClient accountApiClient)
        {
            _logger = logger;
            _accountApiClient = accountApiClient;
        }

        public async Task<IEnumerable<EmployerIdentifier>> GetEmployerIdentifiersAsync(string userId)
        {
            var accounts = await _accountApiClient.GetUserAccounts(userId);

            return accounts
                .Select(acc =>
                    new EmployerIdentifier { AccountId = acc.HashedAccountId, EmployerName = acc.DasAccountName });
        }

        public EmployerIdentifier GetCurrentEmployerAccountId(HttpContext context)
        {
            return (EmployerIdentifier)context.Items[ContextItemKeys.EmployerIdentifier];
        }

        private async Task<string> getUserRole(EmployerIdentifier employerAccount, string userId)
        {
            var accounts = await _accountApiClient.GetAccountUsers(employerAccount.AccountId);

            if (accounts == null || !accounts.Any())
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
                var result = await getUserRole(employerIdentifier, userId);

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
}

