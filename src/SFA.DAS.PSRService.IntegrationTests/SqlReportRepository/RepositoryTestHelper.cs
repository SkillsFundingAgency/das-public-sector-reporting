using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository;

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
            connection.Execute($"delete AuditHistory where ReportId = '{ReportOneId}'");
            connection.Execute($"delete Report where Id = '{ReportOneId}'");

            connection.Execute($"delete AuditHistory where ReportId = '{ReportTwoId}'");
            connection.Execute($"delete Report where Id = '{ReportTwoId}'");
        }

    }
    public static DateTime TrimDateTime(DateTime date) {
        return new DateTime(date.Ticks - (date.Ticks % TimeSpan.TicksPerSecond), date.Kind);
    }

    public static Guid ReportOneId { get; } = new Guid("CDF3F279-3AE1-45A7-B0E6-01B06621853B");
    public static Guid ReportTwoId { get; } = new Guid("B5B28BD5-6B3B-460F-8576-F367483B54C1");


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