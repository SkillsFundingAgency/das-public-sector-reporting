using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using AutoMapper;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests.SubmitReportHandlerTests.Given_Mapped_Dto_Is_Null
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class Given_Mapped_Dto_Is_Null
    :GivenWhenThen<SubmitReportHandler>
    {
        private Mock<IReportRepository> _mockRepository;

        protected override void Given()
        {
            _mockRepository = new Mock<IReportRepository>();

            var mockMapper = new Mock<IMapper>();

            mockMapper
                .Setup(
                    m => m.Map<ReportDto>(It.IsAny<Report>())
                )
                .Returns((ReportDto)null);

            SUT = new SubmitReportHandler(
                mockMapper.Object,
                _mockRepository.Object);
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
    }
}