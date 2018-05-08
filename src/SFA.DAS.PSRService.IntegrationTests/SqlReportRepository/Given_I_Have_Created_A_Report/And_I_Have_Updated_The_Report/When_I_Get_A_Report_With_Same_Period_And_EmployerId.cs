﻿using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_I_Have_Created_A_Report.And_I_Have_Updated_The_Report
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class When_I_Get_A_Report_With_Same_Period_And_EmployerId
    : And_I_Have_Updated_The_Report_Data
    {
        private ReportDto retrievedReport;

        protected override void When()
        {
            retrievedReport
                =
                SUT
                    .Get(
                        CreatedReport.ReportingPeriod
                        , CreatedReport.EmployerId);
        }

        [Test]
        public void Then_The_Retrieved_Report_Data_Is_The_Updated_Data()
        {
            Assert
                .AreEqual(
                    UpdatedReportingData
                    , retrievedReport
                        .ReportingData);
        }
    }
}