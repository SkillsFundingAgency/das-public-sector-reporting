using System;
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
        [JsonRequired]
        public DateTime SubmissionClose { get; set; }
        [JsonRequired]
        public string ApplicationUrl { get; set; }
        [JsonRequired]
        public string RootDomainUrl { get; set; }
        [JsonRequired]
        public string HomeUrl { get; set; }
        [JsonRequired]
        public SessionStoreConfiguration SessionStore { get; set; }
        public TimeSpan? AuditWindowSize { get; set; }
        public NServiceBusConfiguration NServiceBus { get; set; }
    }
}