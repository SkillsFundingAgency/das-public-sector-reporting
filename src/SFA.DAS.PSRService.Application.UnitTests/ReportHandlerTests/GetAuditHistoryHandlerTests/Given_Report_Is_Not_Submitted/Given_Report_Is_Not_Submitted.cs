using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests.GetAuditHistoryHandlerTests.Given_Report_Is_Not_Submitted;

[ExcludeFromCodeCoverage]
[TestFixture]
public class Given_Report_Is_Not_Submitted : GivenWhenThen<GetReportEditHistoryMostRecentFirstHandler>
{
    private Mock<IReportRepository> _mockRepository;
    private readonly Guid _expectedReportId = new("3F0D018B-22DC-45B7-B81B-ED7C5CA024CF");

    protected override void Given() => Sut = new GetReportEditHistoryMostRecentFirstHandler(SetupMockRepositoryReturn(), Mock.Of<IMapper>());

    private IReportRepository SetupMockRepositoryReturn()
    {
        _mockRepository = new Mock<IReportRepository>();

        _mockRepository
            .Setup(m => m.Get(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new ReportDto { Submitted = false, Id = _expectedReportId });

        _mockRepository
            .Setup(m => m.Get(It.IsAny<Guid>()))
            .ReturnsAsync(new ReportDto { Submitted = false, Id = _expectedReportId });

        _mockRepository
            .Setup(m => m.GetAuditRecordsMostRecentFirst(It.IsAny<Guid>()))
            .ReturnsAsync(new List<AuditRecordDto>(0).AsReadOnly);

        return _mockRepository.Object;
    }

    protected override async Task When()
    {
        var request = new GetReportEditHistoryMostRecentFirst(period: Period.FromInstantInPeriod(DateTime.UtcNow), accountId: "SomeEmployerId");

        await Sut.Handle(request, new CancellationToken());
    }

    [Test]
    public void Then_Repository_Is_Queried_For_History_Matching_Report_Id()
    {
        _mockRepository.Verify(m => m.GetAuditRecordsMostRecentFirst(It.Is<Guid>(id => id.Equals(_expectedReportId))));
    }
}