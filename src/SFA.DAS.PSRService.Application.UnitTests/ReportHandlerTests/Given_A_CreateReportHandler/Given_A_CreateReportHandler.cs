using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Microsoft.Extensions.FileProviders;
using Moq;
using SFA.DAS.NServiceBus;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Application.Mapping;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests.Given_A_CreateReportHandler
{
    [ExcludeFromCodeCoverage]
    public abstract class Given_A_CreateReportHandler
        : GivenWhenThen<CreateReportHandler>
    {
        protected Mock<IReportRepository> ReportRepositoryMock;
        protected string EmployerId = "ABCDE";
        protected Period ReportingPeriod = Period.ParsePeriodString("1718");

        protected Mock<IEventPublisher> MockEventPublisher;

        protected override void Given()
        {
            new Mock<IMapper>();
            ReportRepositoryMock = new Mock<IReportRepository>();
            new Mock<IFileProvider>();
            MockEventPublisher = new Mock<IEventPublisher>();

            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddProfile<ReportMappingProfile>();
                    cfg.AddProfile<ReportCreatedProfile>();
                });

            SUT = new CreateReportHandler(
                ReportRepositoryMock.Object,
                config.CreateMapper(),
                new TestQuestionConfigProvider(), 
                MockEventPublisher.Object);
        }
    }
}
