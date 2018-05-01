using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.PSRService.Web.Models;
using Microsoft.AspNetCore.Routing;
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

        public async Task<IDictionary<string, EmployerIdentifier>> GetEmployerIdentifiersAsync(string userId)
        {
        
                var accounts = await _accountApiClient.GetUserAccounts(userId);

                return accounts
                    .Select(acc =>
                        new EmployerIdentifier {AccountId = acc.HashedAccountId, EmployerName = acc.DasAccountName})
                    .ToDictionary(item => item.AccountId);
           
        }

        public EmployerIdentifier GetCurrentEmployerAccountId(HttpContext context)
        {
            return (EmployerIdentifier) context.Items[ContextItemKeys.EmployerIdentifier];
        }
    }
}
