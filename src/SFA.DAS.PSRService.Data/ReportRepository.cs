using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Data
{
    public class ReportRepository : IReportRepository
    {
        private readonly string _connectionString;

        public ReportRepository(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public Task<Report> CreateNewContact(ReportCreateDomainModel newContact)
        {
            throw new NotImplementedException();
        }

        public string Get(Guid reportId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var json = connection.ExecuteScalar<string>("select top 1 ReportingData from Report");
                return json;
            }
        }
    }
}