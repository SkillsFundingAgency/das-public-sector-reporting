﻿using MediatR;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.ReportServiceTests.Submit.Given_A_Valid_Report;

[ExcludeFromCodeCoverage]
public abstract class Given_A_Valid_Report : GivenWhenThen<IReportService>
{
    protected Report ValidNotSubmittedReport;
    protected Mock<IMediator> MockMediator;
    protected Mock<IPeriodService> MockPeriodService;

    protected Given_A_Valid_Report()
    {
        MockMediator = new Mock<IMediator>();
        MockPeriodService = new Mock<IPeriodService>();

        Sut = new ReportService(
            config: Mock.Of<IWebConfiguration>(),
            mediator: MockMediator.Object,
            periodService: MockPeriodService.Object);

        ValidNotSubmittedReport = 
            new ReportBuilder()
                .WithValidSections()
                .WhereReportIsNotAlreadySubmitted()
                .Build();
    }
}