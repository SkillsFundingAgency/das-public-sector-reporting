using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.GovUK.Auth.Employer;

namespace SFA.DAS.PSRService.Application.EmployerUserAccounts.Responses;

public class GetUserAccountsResponse
{
    [JsonProperty]
    public bool IsSuspended { get; set; }
    [JsonProperty] 
    public string EmployerUserId { get; set; }
    [JsonProperty] 
    public string FirstName { get; set; }
    [JsonProperty] 
    public string LastName { get; set; }
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
    [JsonProperty("apprenticeshipEmployerType")]
    public ApprenticeshipEmployerType ApprenticeshipEmployerType { get; set; }
}