using System.Data;
using Azure.Core;
using Azure.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.PSRService.Web.Extensions;

public static class DatabaseExtensions
{
    private const string AzureResource = "https://database.windows.net/";

    private static readonly ChainedTokenCredential AzureServiceTokenProvider = new(
        new ManagedIdentityCredential(),
        new AzureCliCredential(),
        new VisualStudioCodeCredential(),
        new VisualStudioCredential());

    public static void AddDatabaseRegistration(this IServiceCollection services, string sqlConnectionString)
    {
        services.AddScoped<IDbConnection>(_ =>
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(sqlConnectionString);
            var useManagedIdentity = !connectionStringBuilder.IntegratedSecurity && string.IsNullOrEmpty(connectionStringBuilder.UserID);

            if (!useManagedIdentity)
            {
                return new SqlConnection(sqlConnectionString);
            }

            return new SqlConnection
            {
                ConnectionString = sqlConnectionString,
                AccessToken = AzureServiceTokenProvider.GetToken(new TokenRequestContext(scopes: [AzureResource])).Token
            };
        });
    }
}