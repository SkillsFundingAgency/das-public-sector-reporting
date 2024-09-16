using System.Diagnostics.CodeAnalysis;
using MediatR;
using Moq;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.ReportServiceTests.Can_Be_Edited.Given_A_Null_Report;

[ExcludeFromCodeCoverage]
public abstract class Given_A_Null_Report
    :GivenWhenThen<IReportService>
{
    protected Mock<IPeriodService> MockPeriodService;
    protected Report StubReport;

    public Given_A_Null_Report()
    {
        MockPeriodService = new Mock<IPeriodService>();

        Sut = new ReportService(
            config: Mock.Of<IWebConfiguration>(),
            mediator: Mock.Of<IMediator>(),
            periodService: MockPeriodService.Object
        );

        StubReport = null;
    }
}