using System;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Microsoft.Extensions.FileProviders;
using Moq;
using SFA.DAS.NServiceBus;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Application.UnitTests.FileInfo;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Messages.Events;

namespace SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests.Given_A_CreateReportHandler
{
    [ExcludeFromCodeCoverage]
    public abstract class Given_A_CreateReportHandler
        : GivenWhenThen<CreateReportHandler>
    {
        protected Mock<IMapper> MapperMock;
        protected Mock<IReportRepository> ReportRepositoryMock;
        private Mock<IFileProvider> _fileProviderMock;
        protected string EmployerId = "ABCDE";
        protected Period ReportingPeriod = Period.ParsePeriodString("1718");

        protected internal Report MappedReport;

        protected override void Given()
        {
            MapperMock = new Mock<IMapper>();
            ReportRepositoryMock = new Mock<IReportRepository>();
            _fileProviderMock = new Mock<IFileProvider>();

            SUT = new CreateReportHandler(
                ReportRepositoryMock.Object,
                MapperMock.Object,
                _fileProviderMock.Object,
                Mock.Of<IEventPublisher>());

            MappedReport = new Report
            {
                EmployerId = EmployerId,
                ReportingPeriod = ReportingPeriod.PeriodString,
                Id = Guid.NewGuid(),
                Submitted = false
            };
            var fileInfo = new StringFileInfo("", "QuestionConfig.json");

            _fileProviderMock.Setup(s => s.GetFileInfo(It.IsAny<string>())).Returns(fileInfo);
            MapperMock.Setup(s => s.Map<Report>(It.IsAny<ReportDto>())).Returns(MappedReport);
            MapperMock.Setup(m => m.Map<ReportCreated>(It.IsAny<Report>())).Returns(new ReportCreated());
        }
    }
}
