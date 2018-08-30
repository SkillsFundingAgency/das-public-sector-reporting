using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests.GetAuditHistoryHandlerTests.Given_Report_Is_Submitted
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class Given_Report_Is_Not_Submitted
        : GivenWhenThen<GetReportEditHistoryMostRecentFirstHandler>
    {
        private IEnumerable<AuditRecord> auditItems;

        protected override void Given()
        {
            SUT = new GetReportEditHistoryMostRecentFirstHandler(
                SetupMockRepositoryReturn(),
                Mock.Of<IMapper>());
        }

        private IReportRepository SetupMockRepositoryReturn()
        {
            var mockRepository = new Mock<IReportRepository>();

            mockRepository
                .Setup(
                    m => m.Get(
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                .Returns(
                    new ReportDto {Submitted = true});

            mockRepository
                .Setup(
                    m => m.Get(
                        It.IsAny<Guid>()))
                .Returns(
                    new ReportDto { Submitted = true });

            return
                mockRepository
                    .Object;
        }

        protected override void When()
        {
            var requrestWithSubmittedReport =
                new GetReportEditHistoryMostRecentFirst(
                    period: Period.FromInstantInPeriod(DateTime.UtcNow), 
                    accountId:"SomeAccountId");

            auditItems = SUT.Handle(requrestWithSubmittedReport, new CancellationToken()).Result;
        }

        [Test]
        public void Then_Empty_Collection_Is_Returned()
        {
            auditItems.Should().BeEmpty();
        }
    }
}