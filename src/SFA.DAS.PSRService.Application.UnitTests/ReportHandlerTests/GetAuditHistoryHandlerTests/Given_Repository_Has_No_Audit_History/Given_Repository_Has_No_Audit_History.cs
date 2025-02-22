﻿using System;
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

namespace SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests.GetAuditHistoryHandlerTests.Given_Repository_Has_No_Audit_History;

[ExcludeFromCodeCoverage]
[TestFixture]
public class Given_Repository_Has_No_Audit_History : GivenWhenThen<GetReportEditHistoryMostRecentFirstHandler>
{
    private IEnumerable<AuditRecord> _auditItems;

    protected override void Given()
    {
        Sut = new GetReportEditHistoryMostRecentFirstHandler(SetupMockRepositoryReturn(), Mock.Of<IMapper>());
    }

    protected override async Task When()
    {
        var anyOldRequest = new GetReportEditHistoryMostRecentFirst(period:Period.FromInstantInPeriod(DateTime.UtcNow), accountId:"SomeAccountId");

        _auditItems = await Sut.Handle(anyOldRequest, new CancellationToken());
    }
    private static IReportRepository SetupMockRepositoryReturn()
    {
        var mockRepository = new Mock<IReportRepository>();

        mockRepository
            .Setup(m => m.GetAuditRecordsMostRecentFirst(It.IsAny<Guid>()))
            .ReturnsAsync(new List<AuditRecordDto>(0).AsReadOnly);

        return mockRepository.Object;
    }
    [Test]
    public void Then_Empty_Collection_Is_Returned()
    {
        _auditItems.Should().BeEmpty();
    }
}