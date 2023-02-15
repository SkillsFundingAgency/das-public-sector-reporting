using System.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Services.AppAuthentication;
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
                    var azureServiceTokenProvider = new AzureServiceTokenProvider();
                    var accessToken = azureServiceTokenProvider.GetAccessTokenAsync(AzureResource).Result;
                    return new SqlConnection
                    {
                        ConnectionString = sqlConnectionString,
                        AccessToken = accessToken,
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