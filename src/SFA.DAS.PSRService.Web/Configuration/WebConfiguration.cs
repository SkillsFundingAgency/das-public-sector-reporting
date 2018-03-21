using Newtonsoft.Json;
using SFA.DAS.EAS.Account.Api.Client;

namespace SFA.DAS.PSRService.Web.Configuration
{
    public class WebConfiguration : IWebConfiguration
    {
        [JsonRequired]
        public IdentityServerConfiguration Identity { get; set; }
        [JsonRequired]
        public AccountApiConfiguration AccountsApi { get; set; }
        [JsonRequired]
        public string SqlConnectionString { get; set; }
    }
}