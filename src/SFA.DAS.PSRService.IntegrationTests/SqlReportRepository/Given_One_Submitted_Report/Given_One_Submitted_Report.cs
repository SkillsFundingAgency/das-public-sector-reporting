using System;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Data;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_One_Submitted_Report
{
    [ExcludeFromCodeCoverage]
    public abstract class Given_One_Submitted_Report
    : GivenWhenThen<IReportRepository>
    {
        protected ReportDto SubmittedReport;
        protected string EmployerId = "TestEmployerID";

        protected override void Given()
        {
            RepositoryTestHelper
                .ClearData();

            SUT = new SQLReportRepository(new SqlConnection(RepositoryTestHelper.ConnectionString));

            SubmittedReport = new ReportDto
            {
                Id = RepositoryTestHelper.ReportOneId,
                EmployerId = EmployerId,
                ReportingData = "Some dumb piece of json",
                ReportingPeriod = "2222",
                Submitted = true
            };

            SUT
                .Create(
                    SubmittedReport);
        }

        [TearDown]
        public void ClearDatabase()
        {
            RepositoryTestHelper
                .ClearData();
        }
    }
}