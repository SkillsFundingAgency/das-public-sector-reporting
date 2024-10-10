using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.PSRService.Application.OuterApi;

namespace SFA.DAS.PSRService.Web.Configuration;

public interface IWebConfiguration
{
    AccountApiConfiguration AccountsApi { get; set; }
    string SqlConnectionString { get; set; }
    string RootDomainUrl { get; set; }
    string EmployerCommitmentsV2BaseUrl { get; set; }
    string DataProtectionKeysDatabase { get; set; }
    SessionStoreConfiguration SessionStore { get; set; }
    TimeSpan? AuditWindowSize { get; set; }
    ZenDeskConfiguration ZenDeskConfig { get; set; }
    OuterApiConfiguration OuterApiConfiguration { get; set; }
}