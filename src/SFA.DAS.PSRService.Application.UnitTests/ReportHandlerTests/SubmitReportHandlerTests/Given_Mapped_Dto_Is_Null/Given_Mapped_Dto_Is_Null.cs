using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using AutoMapper;
using Moq;
using NUnit.Framework;
using SFA.DAS.NServiceBus;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Messages.Events;

namespace SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests.SubmitReportHandlerTests.Given_Mapped_Dto_Is_Null
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class Given_Mapped_Dto_Is_Null
    :GivenWhenThen<SubmitReportHandler>
    {
        private Mock<IReportRepository> _mockRepository;
        private Mock<IEventPublisher> _mockEventPublisher;

        protected override void Given()
        {
            _mockRepository = new Mock<IReportRepository>();
            _mockEventPublisher = new Mock<IEventPublisher>();

            var mockMapper = new Mock<IMapper>();

            mockMapper
                .Setup(
                    m => m.Map<ReportDto>(It.IsAny<Report>())
                )
                .Returns((ReportDto)null);

            SUT = new SubmitReportHandler(
                mockMapper.Object,
                _mockRepository.Object,
                _mockEventPublisher.Object);
        }

        protected override void When()
        {
            SUT
                .Handle(
                    new SubmitReportRequest(new Report()),
                    new CancellationToken());
        }

        [Test]
        public void Then_ReportRepositoryUpdate_Is_Not_Called()
        {
            _mockRepository
                .Verify(
                    m => m.Update(It.IsAny<ReportDto>()),
                    Times.Never);
        }

        [Test]
        public void Then_ReportRepositoryDeleteHistory_Is_Not_Called()
        {
            _mockRepository
                .Verify(
                    m => m.DeleteHistory(It.IsAny<Guid>()),
                    Times.Never);
        }

        [Test]
        public void Then_ReportSubmitted_Event_Is_Not_Published()
        {
            _mockEventPublisher
                .Verify(
                    m => m.Publish(
                        It.IsAny<ReportSubmitted>()),
                    Times.Never);
        }
    }
}