﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Dapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
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



        public ReportDto Get(string period, string employerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {


                var json = connection.Query<ReportDto>("select top 1 Id,EmployerId, ReportingPeriod, ReportingData, Submitted from Report where EmployerID = @EmployerId and ReportingPeriod = @ReportingPeriod", new { EmployerId = employerId, ReportingPeriod = period }).FirstOrDefault();

                return json;
            }
        }

        public IEnumerable<ReportDto> GetSubmitted(string employerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var reportData = connection.Query<ReportDto>("select * from Report where EmployerID = @EmployerId and Submitted = 1", new { EmployerId = employerId });


                return reportData.ToList();
            }
        }

        public ReportDto Create(ReportDto report)
        {
           
            
         

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var reportInsertStatus = connection.Execute("INSERT INTO [dbo].[Report]([Id],[EmployerId],[ReportingPeriod],[ReportingData],[Submitted])VALUES(@Id, @EmployerId, @ReportingPeriod, @ReportingData, @Submitted)", new { Id = report.Id, EmployerId = report.EmployerId, ReportingData = report.ReportingData, ReportingPeriod = report.ReportingPeriod, Submitted = report.Submitted });

                    if (reportInsertStatus != 1)
                        throw new Exception("Unable to create new report");
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            
               
            }

            return report;
        }

        public ReportDto Update(ReportDto reportDto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var reportInsertStatus = connection.Execute("UPDATE [dbo].[Report] SET [ReportingData] = @ReportingData,[Submitted] = @Submitted where Id = @Id",new {ReportingData = reportDto.ReportingData, Submitted = reportDto.Submitted, Id = reportDto.Id });

                    if (reportInsertStatus != 1)
                        throw new Exception("Unable to update report");
                }
                catch (Exception ex)
                {

                    throw ex;
                }


            }

            return reportDto;
        }
    }
}