using System.Diagnostics.CodeAnalysis;
using Moq;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.ReportServiceTests.Submit.Given_A_Valid_Report;

[ExcludeFromCodeCoverage]
public abstract class And_Report_Is_For_This_Period : And_Report_Is_Not_Submitted
{
    protected And_Report_Is_For_This_Period()
    {
        MockPeriodService.Setup(m => m.PeriodIsCurrent(It.IsAny<Period>()))
            .Returns(true);
    }
}