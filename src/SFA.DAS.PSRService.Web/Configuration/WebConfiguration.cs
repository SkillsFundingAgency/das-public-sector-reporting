using Newtonsoft.Json;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.PSRService.Application.OuterApi;

namespace SFA.DAS.PSRService.Web.Configuration;

public class WebConfiguration : IWebConfiguration
{
    [JsonRequired]
    public IdentityServerConfiguration Identity { get; set; }
    [JsonRequired]
    public AccountApiConfiguration AccountsApi { get; set; }
    [JsonRequired]
    public string SqlConnectionString { get; set; }
    [JsonRequired]
    public string RootDomainUrl { get; set; }
    [JsonRequired]
    public string EmployerCommitmentsV2BaseUrl { get; set; }
    [JsonRequired]
    public string DataProtectionKeysDatabase { get; set; }
    [JsonRequired]
    public SessionStoreConfiguration SessionStore { get; set; }
    public TimeSpan? AuditWindowSize { get; set; }
    public ZenDeskConfiguration ZenDeskConfig { get; set; }      
    [JsonRequired]
    public OuterApiConfiguration OuterApiConfiguration { get; set; }
    public bool UseGovSignIn { get; set; }
}