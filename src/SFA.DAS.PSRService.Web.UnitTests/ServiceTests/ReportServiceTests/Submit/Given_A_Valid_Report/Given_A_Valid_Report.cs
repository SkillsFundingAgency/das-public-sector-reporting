using System.Diagnostics.CodeAnalysis;
using MediatR;
using Moq;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Services;
using SFA.DAS.PSRService.Web.UnitTests.ServiceTests.PeriodServiceTests;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.ReportServiceTests.Submit.Given_A_Valid_Report
{
    [ExcludeFromCodeCoverage]
    public abstract class Given_A_Valid_Report
    : GivenWhenThen<IReportService>
    {
        protected Report ValidNotSubmittedReport;
        protected Mock<IMediator> MockMediator;
        protected Mock<IPeriodService> MockPeriodService;

        public Given_A_Valid_Report()
        {
            MockMediator = new Mock<IMediator>();
            MockPeriodService = new Mock<IPeriodService>();

            SUT = new ReportService(
               config: Mock.Of<IWebConfiguration>(),
                mediator: MockMediator.Object,
                periodService: MockPeriodService.Object);

            ValidNotSubmittedReport = new ReportBuilder().BuildReportWithValidSections();
        }
    }
}