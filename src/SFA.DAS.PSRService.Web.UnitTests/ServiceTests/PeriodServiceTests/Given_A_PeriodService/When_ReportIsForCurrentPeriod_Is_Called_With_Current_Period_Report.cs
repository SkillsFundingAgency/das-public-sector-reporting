﻿using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.PeriodServiceTests.Given_A_PeriodService
{
    [ExcludeFromCodeCoverage]
    [TestFixture(2017, 9, 30)]
    [TestFixture(2017, 10, 1)]
    [TestFixture(2017, 4, 1)]
    [TestFixture(2017, 3, 31)]
    public sealed class When_ReportIsForCurrentPeriod_Is_Called_With_Current_Period_Report
    :Given_A_PeriodService
    {
        private bool reportIsForCurrentPeriod;

        public When_ReportIsForCurrentPeriod_Is_Called_With_Current_Period_Report(int year, int month, int day) : base(year, month, day)
        {
            var report
                =
                new ReportBuilder()
                    .BuildReportWithValidSections();

            report.Period = Period.FromInstantInPeriod(CurrentInstant);

            reportIsForCurrentPeriod = SUT.ReportIsForCurrentPeriod(report);
        }

        [Test]
        public void Then_True_Is_Returned()
        {
            reportIsForCurrentPeriod
                .Should()
                .BeTrue("Report is for current period");
        }
    }
}