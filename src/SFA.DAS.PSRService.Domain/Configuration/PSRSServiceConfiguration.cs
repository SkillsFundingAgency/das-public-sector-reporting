using SFA.DAS.PSRService.Domain.Interfaces;

namespace SFA.DAS.PSRService.Domain.Configuration
{
    public class PSRSServiceConfiguration : IConfiguration
    {
        public string DatabaseConnectionString { get; set; }
        public string ServiceBusConnectionString { get; set; }
        public IdentityServerConfiguration Identity { get; set; }
        public string DashboardUrl { get; set; }
        public string Hashstring { get; set; }
        public string AllowedHashstringCharacters { get; set; }
    }
}
