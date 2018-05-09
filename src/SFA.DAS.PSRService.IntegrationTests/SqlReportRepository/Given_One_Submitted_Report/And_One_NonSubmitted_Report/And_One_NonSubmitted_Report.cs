using System;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.PSRService.Application.Domain;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_One_Submitted_Report.And_One_NonSubmitted_Report
{
    [ExcludeFromCodeCoverage]
    public abstract class And_One_NonSubmitted_Report
    : Given_One_Submitted_Report
    {
        private ReportDto nonsubmittedReport;

        protected override void Given()
        {
            base.Given();

            nonsubmittedReport = new ReportDto
            {
                Id = Guid.NewGuid(),
                EmployerId = EmployerId,
                ReportingData = "Some junk piece of json",
                ReportingPeriod = "Noo!",
                Submitted = false
            };

            SUT
                .Create(
                    nonsubmittedReport);
        }
    }
}