using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_No_Existing_Reports_In_Database
{
    [ExcludeFromCodeCoverage]
    public sealed class When_I_Call_GetSubmitted
    :Given_No_Existing_Reports_In_Database
    {
        private IList<ReportDto> _retrievedSubmittedReports;

        protected override void When()
        {
            base.When();

            _retrievedSubmittedReports
                =
                SUT
                    .GetSubmitted(
                        "DummyEmployerId");
        }

        [Test]
        public void Then_An_Empty_Collection_Is_Returned()
        {
            Assert
                .IsNotNull(
                    _retrievedSubmittedReports);

            Assert
                .IsEmpty(
                    _retrievedSubmittedReports);
        }
    }
}