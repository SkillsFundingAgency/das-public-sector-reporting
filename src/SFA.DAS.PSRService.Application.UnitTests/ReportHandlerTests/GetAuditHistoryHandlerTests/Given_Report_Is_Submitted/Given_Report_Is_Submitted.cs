using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests.GetAuditHistoryHandlerTests.Given_Report_Is_Submitted;

[ExcludeFromCodeCoverage]
[TestFixture]
public class Given_Report_Is_Not_Submitted : GivenWhenThen<GetReportEditHistoryMostRecentFirstHandler>
{
    private IEnumerable<AuditRecord> _auditItems;

    protected override void Given()
    {
        Sut = new GetReportEditHistoryMostRecentFirstHandler(SetupMockRepositoryReturn(), Mock.Of<IMapper>());
    }

    private static IReportRepository SetupMockRepositoryReturn()
    {
        var mockRepository = new Mock<IReportRepository>();

        mockRepository
            .Setup(m => m.Get(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new ReportDto { Submitted = true });

        mockRepository
            .Setup(m => m.Get(It.IsAny<Guid>()))
            .ReturnsAsync(new ReportDto { Submitted = true });

        return mockRepository.Object;
    }

    protected override async Task When()
    {
        var request = new GetReportEditHistoryMostRecentFirst(period: Period.FromInstantInPeriod(DateTime.UtcNow), accountId: "SomeAccountId");

        _auditItems = await Sut.Handle(request, new CancellationToken());
    }

    [Test]
    public void Then_Empty_Collection_Is_Returned()
    {
        _auditItems.Should().BeEmpty();
    }
}