using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.PSRService.Application.EmployerUserAccounts.Responses
{
    public class GetUserAccountsResponse
    {
        [JsonProperty("UserAccounts")]
        public List<EmployerIdentifier> UserAccounts { get; set; }
    }
    
    public class EmployerIdentifier
    {
        [JsonProperty("EncodedAccountId")]
        public string AccountId { get; set; }
        [JsonProperty("DasAccountName")]
        public string EmployerName { get; set; }
        [JsonProperty("Role")]
        public string Role { get; set; }
    }
}