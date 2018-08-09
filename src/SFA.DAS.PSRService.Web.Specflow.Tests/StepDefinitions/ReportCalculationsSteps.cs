using System;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.PSRService.Web.Specflow.Tests.Repository;
using SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
    [ExcludeFromCodeCoverage]
    [Binding]
    public class ReportCalculationsSteps : BaseTest
    {
        private readonly SQLReportRepository _reportRepository;

        public ReportCalculationsSteps(SQLReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [Then(@"(.*) is saved as reporting percentages employment starts")]
        public void ThenIsSavedAsReportingPercentagesEmploymentStarts(decimal expectedEmploymentStarts)
        {
            ReportVerifier
                .VerifyReport(GetCurrentReport())
                .ReportingPercentages
                .EmploymentStarts
                .Is(expectedEmploymentStarts.ToString("0.00"));
        }

        [Then(@"(.*) is saved as reporting percentages total head count")]
        public void ThenIsSavedAsReportingPercentagesTotalHeadCount(decimal expectedTotalHeadCount)
        {
            ReportVerifier
                .VerifyReport(GetCurrentReport())
                .ReportingPercentages
                .TotalHeadCount
                .Is(expectedTotalHeadCount.ToString("0.00"));
        }

        [Then(@"(.*) is saved as reporting percentages new this period")]
        public void ThenIsSavedAsReportingPercentagesNewThisPeriod(decimal expectedNewThisPeriod)
        {
            ReportVerifier
                .VerifyReport(GetCurrentReport())
                .ReportingPercentages
                .NewThisPeriod
                .Is(expectedNewThisPeriod.ToString("0.00"));
        }

        private ReportDto GetCurrentReport()
        {
            return
                _reportRepository
                    .GetReportWithId(
                        ScenarioContext
                            .Current
                            .Get<Guid>(
                                ContextKeys.CurrentReportID));
        }
    }
}