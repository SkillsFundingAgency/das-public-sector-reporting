using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository
{
    internal static class RepositoryTestHelper
    {
        public static string ConnectionString { get; }

        static RepositoryTestHelper()
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

        public static IEnumerable<AuditRecordDto> GetAllAuditHistory()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                return connection.Query<AuditRecordDto>("select * from AuditHistory");
            }
        }

        public static void ClearData()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("if exists(select 1 from AuditHistory) truncate table AuditHistory");
                connection.Execute("if exists(select 1 from Report) delete from Report");
            }

        }
        public static DateTime TrimDateTime(DateTime date) {
            return new DateTime(date.Ticks - (date.Ticks % TimeSpan.TicksPerSecond), date.Kind);
        }

        public static void AssertReportsAreEquivalent(ReportDto expectedReport, ReportDto actualReport)
        {
            Assert.AreEqual(expectedReport.Id, actualReport.Id);
            Assert.AreEqual(expectedReport.EmployerId, actualReport.EmployerId);
            Assert.AreEqual(expectedReport.ReportingData, actualReport.ReportingData);
            Assert.AreEqual(expectedReport.ReportingPeriod, actualReport.ReportingPeriod);
            Assert.AreEqual(expectedReport.Submitted, actualReport.Submitted);
            Assert.AreEqual(expectedReport.AuditWindowStartUtc, actualReport.AuditWindowStartUtc);
            Assert.AreEqual(expectedReport.UpdatedUtc, actualReport.UpdatedUtc);
            Assert.AreEqual(expectedReport.UpdatedBy, actualReport.UpdatedBy);
        }
    }
}
