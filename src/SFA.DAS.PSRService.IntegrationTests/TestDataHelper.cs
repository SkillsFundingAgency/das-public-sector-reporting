using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using SFA.DAS.PSRService.Application.Domain;

namespace SFA.DAS.PSRService.IntegrationTests
{
    internal static class TestDataHelper
    {
        public static string ConnectionString { get; }

        static TestDataHelper()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            ConnectionString = config["ConnectionString"];
        }

        public static IList<ReportDto> GetAllReports()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                return connection.Query<ReportDto>("select * from Report").ToList();
            }
        }

        public static void ClearData()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("if exists(select 1 from Report) delete from Report");
            }

        }
    }
}
