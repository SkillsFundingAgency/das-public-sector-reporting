using System.Diagnostics.CodeAnalysis;
using MediatR;
using Moq;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Services;
using SFA.DAS.PSRService.Web.UnitTests.ServiceTests.PeriodServiceTests;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.ReportServiceTests.Submit.Given_A_Submitted_Report;

[ExcludeFromCodeCoverage]
public abstract class Given_A_Submitted_Report
    :GivenWhenThen<IReportService>
{
    protected Report AlreadySubmittedReport;

    public Given_A_Submitted_Report()
    {
        Sut = new ReportService(
            config: Mock.Of<IWebConfiguration>(),
            mediator: Mock.Of<IMediator>(),
            periodService: Mock.Of<IPeriodService>());

        AlreadySubmittedReport = 
            new ReportBuilder()
                .WithValidSections()
                .WhereReportIsAlreadySubmitted()
                .Build();
    }      
}