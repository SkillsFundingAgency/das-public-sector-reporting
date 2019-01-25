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
    public class Given_Mapped_Dto_Is_Not_Null
        : GivenWhenThen<UnSubmitReportHandler>
    {
        private Mock<IReportRepository> _mockRepository;

        private ReportDto _mappedDto;

        protected override void Given()
        {
            _mockRepository = new Mock<IReportRepository>();

            var mockMapper = new Mock<IMapper>();

            _mappedDto = new ReportDto();

            _mappedDto.Submitted = false;

            _mappedDto.Id = new Guid("33F2BAD1-F368-4467-A249-D2B936284458");

            mockMapper
                .Setup(
                    m => m.Map<ReportDto>(It.IsAny<Report>())
                )
                .Returns(_mappedDto);

            SUT = new UnSubmitReportHandler(
                mockMapper.Object,
                _mockRepository.Object);
        }

        protected override void When()
        {
            SUT
                .Handle(
                    new UnSubmitReportRequest(new Report()),
                    new CancellationToken());
        }

        [Test]
        public void Then_RepositoryUpdate_Is_Called_With_Mapped_Dto()
        {
            _mockRepository
                .Verify(
                    m => m.Update(
                        It.Is<ReportDto>( r => ReferenceEquals(r, _mappedDto))));
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