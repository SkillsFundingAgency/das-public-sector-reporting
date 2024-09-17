using System.Data;
using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using StructureMap;

namespace SFA.DAS.PSRService.Web.Extensions
{
    public static class DatabaseExtensions
    {
        private const string AzureResource = "https://database.windows.net/";

        public static void AddDatabaseRegistration(this ConfigurationExpression config, IWebHostEnvironment environment, string sqlConnectionString)
        {
            config.For<IDbConnection>().Use($"Build IDbConnection", c => {

                var connectionStringBuilder = new SqlConnectionStringBuilder(sqlConnectionString);
                bool useManagedIdentity = !connectionStringBuilder.IntegratedSecurity && string.IsNullOrEmpty(connectionStringBuilder.UserID);
                if (useManagedIdentity)
                {
                    var azureServiceTokenProvider = new ChainedTokenCredential(
                        new ManagedIdentityCredential(),
                        new AzureCliCredential(),
                        new VisualStudioCodeCredential(),
                        new VisualStudioCredential());

                    return new SqlConnection
                    {
                        ConnectionString = sqlConnectionString,
                        AccessToken = azureServiceTokenProvider.GetToken(new TokenRequestContext(scopes: new string[] { AzureResource })).Token
                    };
                    
                }
                else
                {
                    return new SqlConnection(sqlConnectionString);
                }
            });
        }
    }
}