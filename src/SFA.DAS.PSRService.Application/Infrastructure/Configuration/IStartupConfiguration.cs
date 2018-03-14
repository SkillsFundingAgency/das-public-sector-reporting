using SFA.DAS.PSRService.Domain.Configuration;

namespace SFA.DAS.PSRService.Application.Infrastructure.Configuration
{
    public interface IStartupConfiguration
    {
        string AllowedHashstringCharacters { get; set; }
        string DashboardUrl { get; set; }
        string DatabaseConnectionString { get; set; }
        string Hashstring { get; set; }
        IdentityServerConfiguration Identity { get; set; }
        string TokenCertificateThumbprint { get; set; }
        //string ServiceBusConnectionString { get; set; }
    }
}
