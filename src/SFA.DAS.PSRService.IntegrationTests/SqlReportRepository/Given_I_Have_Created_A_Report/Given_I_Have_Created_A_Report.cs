using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Data;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_I_Have_Created_A_Report
{
    [ExcludeFromCodeCoverage]
    public abstract class Given_I_Have_Created_A_Report
        : Given_A_SQLReportRepository
    {
        protected ReportDto CreatedReport { get; set; }

        protected override void Given()
        {
            base.Given();

            CreatedReport = new ReportDto
            {
                Id = RepositoryTestHelper.ReportOneId,
                EmployerId = "Uncle Bob",
                ReportingData = "Original created data",
                ReportingPeriod = "2222",
                Submitted = true,
                AuditWindowStartUtc = RepositoryTestHelper.TrimDateTime(DateTime.UtcNow.AddMinutes(-1)),
                UpdatedUtc = RepositoryTestHelper.TrimDateTime(DateTime.Now.AddSeconds(-30)),
                UpdatedBy = $"{{ Id: '{Guid.NewGuid()}', Name: 'Name'}}"
            };

            SUT.Create(CreatedReport);
        }


        [TearDown]
        public void ClearDatabase()
        {
            RepositoryTestHelper
                .ClearData();
        }
    }
}