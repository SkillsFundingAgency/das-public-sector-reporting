﻿using System.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using StructureMap;

namespace SFA.DAS.PSRService.Web.Extensions
{
    public static class DatabaseExtensions
    {
        private const string AzureResource = "https://database.windows.net/";

        public static void AddDatabaseRegistration(this ConfigurationExpression config, IWebHostEnvironment environment, string sqlConnectionString)
        {
            config.For<IDbConnection>().Use($"Build IDbConnection", c => {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                return environment.IsDevelopment()
                    ? new SqlConnection(sqlConnectionString)
                    : new SqlConnection
                    {
                        ConnectionString = sqlConnectionString,
                        AccessToken = azureServiceTokenProvider.GetAccessTokenAsync(AzureResource).Result
                    };
            });
        }
    }
}