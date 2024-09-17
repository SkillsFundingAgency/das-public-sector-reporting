using System.Data;
using Azure.Core;
using Azure.Identity;
using Microsoft.Data.SqlClient;
using StructureMap;

namespace SFA.DAS.PSRService.Web.Extensions;

public static class DatabaseExtensions
{
    private const string AzureResource = "https://database.windows.net/";
    
    private static readonly ChainedTokenCredential AzureServiceTokenProvider = new(
        new ManagedIdentityCredential(),
        new AzureCliCredential(),
        new VisualStudioCodeCredential(),
        new VisualStudioCredential());

    public static void AddDatabaseRegistration(this ConfigurationExpression config, string sqlConnectionString)
    {
        config.For<IDbConnection>().Use($"Build IDbConnection", c => {

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