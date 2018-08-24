using System.Diagnostics.CodeAnalysis;
using Moq;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.ReportServiceTests.Given_A_Not_Submitted_Report.And_Report_Is_For_Current_Period
{
    [ExcludeFromCodeCoverage]
    public abstract class And_Report_Is_For_Current_Period
    :Given_A_Not_Submitted_Report
    {
        public And_Report_Is_For_Current_Period()
        {
            MockPeriodService
                .Setup(
                    m =>
                        m.ReportIsForCurrentPeriod(
                            It.IsAny<Report>()
                        ))
                .Returns(true);
        }
    }
}