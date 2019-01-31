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

namespace SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests.UnSubmitReportHandlerTests.Given_Mapped_Dto_Is_Not_Null
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class Given_Report_Exists
        : GivenWhenThen<UnSubmitReportHandler>
    {
        private Mock<IReportRepository> _mockRepository;

        private ReportDto _mappedDto;

        protected override void Given()
        {
            _mockRepository = new Mock<IReportRepository>();

            _mockRepository
                .Setup(
                    m => m.Get(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReportDto());

            SUT = new UnSubmitReportHandler(_mockRepository.Object);
        }

        protected override void When()
        {
            SUT
                .Handle(
                    new UnSubmitReportRequest("123456", Period.ParsePeriodString("1617")),
                    new CancellationToken());
        }

        [Test]
        public void Then_Dto_Passed_To_RepositoryUpdate_Has_UnSubmitted_Status()
        {
           _mockRepository
               .Verify(
                   m => m.Update(
                       It.Is<ReportDto>( r => r.Submitted == false)));
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