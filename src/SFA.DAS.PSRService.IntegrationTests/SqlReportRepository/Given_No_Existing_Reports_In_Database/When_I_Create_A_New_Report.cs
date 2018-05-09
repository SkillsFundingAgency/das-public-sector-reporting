using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_No_Existing_Reports_In_Database
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class When_I_Create_A_New_Report
    : Given_No_Existing_Reports_In_Database
    {
        private ReportDto reportCreatedViaRepository;

        protected override void When()
        {
            reportCreatedViaRepository = new ReportDto
            {
                Id = Guid.NewGuid(),
                EmployerId = "Uncle Sam",
                ReportingData = "Some genious piece of json",
                ReportingPeriod = "1111",
                Submitted = false
            };

            SUT
                .Create(
                    reportCreatedViaRepository);
        }

        [Test]
        public void Then_Report_Is_Persisted_To_Underlying_Data_Store()
        {
            var reportPersistedToDB
                =
                RepositoryTestHelper
                    .GetAllReports()
                    .Single();

            RepositoryTestHelper
                .AssertReportsAreEquivalent(
                    reportCreatedViaRepository
                    , reportPersistedToDB
                    );
        }
    }
}