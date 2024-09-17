using MediatR;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.ReportServiceTests.Can_Be_Edited.Given_A_Not_Submitted_Report;

[ExcludeFromCodeCoverage]
public abstract class Given_A_Not_Submitted_Report : GivenWhenThen<IReportService>
{
    protected readonly Mock<IPeriodService> MockPeriodService;
    protected readonly Report StubReport;

    protected Given_A_Not_Submitted_Report()
    {
        MockPeriodService = new Mock<IPeriodService>();

        Sut = new ReportService(
            config: Mock.Of<IWebConfiguration>(),
            mediator: Mock.Of<IMediator>(),
            periodService: MockPeriodService.Object
        );

        StubReport = new Report {Submitted = false};
    }
}