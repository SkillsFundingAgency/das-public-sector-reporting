using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_No_Existing_Reports_In_Database
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class When_I_Create_A_New_Report : Given_No_Existing_Reports_In_Database
    {
        private ReportDto _reportCreatedViaRepository;

        protected override void When()
        {
            _reportCreatedViaRepository = new ReportDto
            {
                Id = Guid.NewGuid(),
                EmployerId = "Uncle Sam",
                ReportingData = "Some genious piece of json",
                ReportingPeriod = "1111",
                Submitted = false,
                AuditWindowStartUtc = DateTime.UtcNow,
                UpdatedUtc = DateTime.UtcNow,
                UpdatedBy = "{ FullName: 'Bob Shurunkle'}"
            };

            SUT
                .Create(
                    _reportCreatedViaRepository);
        }

        [Test]
        public void Then_Report_Is_Persisted_To_Underlying_Data_Store()
        {
            var reportPersistedToDb
                =
                RepositoryTestHelper
                    .GetAllReports()
                    .Single();

            RepositoryTestHelper
                .AssertReportsAreEquivalent(
                    _reportCreatedViaRepository
                    , reportPersistedToDb
                    );
        }
    }
}