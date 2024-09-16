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

namespace SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests.SubmitReportHandlerTests.Given_Mapped_Dto_Is_Not_Null;

[ExcludeFromCodeCoverage]
[TestFixture]
public class Given_Mapped_Dto_Is_Not_Null : GivenWhenThen<SubmitReportHandler>
{
    private Mock<IReportRepository> _mockRepository;
    private ReportDto _mappedDto;

    protected override void Given()
    {
        _mockRepository = new Mock<IReportRepository>();

        var mockMapper = new Mock<IMapper>();

        _mappedDto = new ReportDto
        {
            Submitted = false,
            Id = new Guid("33F2BAD1-F368-4467-A249-D2B936284458")
        };

        mockMapper
            .Setup(m => m.Map<ReportDto>(It.IsAny<Report>()))
            .Returns(_mappedDto);

        Sut = new SubmitReportHandler(mockMapper.Object, _mockRepository.Object);
    }

    protected override void When()
    {
        Sut.Handle(new SubmitReportRequest(new Report()), new CancellationToken());
    }

    [Test]
    public void Then_RepositoryUpdate_Is_Called_With_Mapped_Dto()
    {
        _mockRepository.Verify(m => m.Update(It.Is<ReportDto>( r => ReferenceEquals(r, _mappedDto))));
    }

    [Test]
    public void Then_Dto_Passed_To_RepositoryUpdate_Has_Submitted_Status()
    {
        _mockRepository.Verify(m => m.Update(It.Is<ReportDto>( r => r.Submitted == true)));
    }

    [Test]
    public void Then_RepositoryDeleteHistory_Is_Called()
    {
        _mockRepository.Verify(m => m.DeleteHistory(It.IsAny<Guid>()), Times.Once);
    }

    [Test]
    public void Then_RepositoryDeleteHistory_Is_Called_With_Id_Of_Mapped_Dto()
    {
        _mockRepository.Verify(m => m.DeleteHistory(It.Is<Guid>( g => g.Equals(_mappedDto.Id))));
    }
}