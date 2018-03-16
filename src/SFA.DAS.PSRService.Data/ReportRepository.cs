using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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



        public ReportDto Get(string period, long employerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                

                var json = connection.Query<ReportDto>("select top 1 Id,EmployerId, ReportingPeriod, ReportingData, Submitted from Report").FirstOrDefault();
                   
                return json;
            }
        }

        public IEnumerable<ReportDto> GetSubmitted(long employerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var reportData = connection.Query<ReportDto>("select * from Report where EmployerID = @EmployerId and Submitted = 1", new {EmployerId = employerId});
                
            
                return reportData;
            }
        }

      
       
    }
}