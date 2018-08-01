using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Repository
{
    public class SQLReportRepository
    {
        private readonly string _connectionString;

        public SQLReportRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ReportDto Get(string period, string employerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var report = connection.QuerySingleOrDefault<ReportDto>("select * from Report where EmployerID = @employerId and ReportingPeriod = @period",
                    new {employerId, period});
                return report;
            }
        }

        public ReportDto GetReportWithId(Guid reportId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var report = connection.QuerySingleOrDefault<ReportDto>("select * from Report where ID = @reportId",
                    new { reportId });

                return report;
            }
        }

        public IList<ReportDto> GetSubmitted(string employerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var reportData = connection.Query<ReportDto>("select * from Report where EmployerID = @employerId and Submitted = 1",
                    new {employerId});
                return reportData.ToList();
            }
        }

        public void Create(ReportDto report)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(@"
                    INSERT INTO [dbo].[Report] ([Id],[EmployerId],[ReportingPeriod],[ReportingData],[Submitted])
                                        VALUES (@Id, @EmployerId, @ReportingPeriod, @ReportingData, @Submitted)",
                    new {report.Id, report.EmployerId, report.ReportingData, report.ReportingPeriod, report.Submitted});
            }
        }

        public void Update(ReportDto reportDto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("UPDATE [dbo].[Report] SET [ReportingData] = @ReportingData,[Submitted] = @Submitted where Id = @Id",
                    new {reportDto.ReportingData, reportDto.Submitted, reportDto.Id});
            }
        }

        public void Delete(string employerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("DELETE FROM [dbo].[Report] WHERE [employerId] = @employerId",
                    new { employerId });
            }
        }
    }
}