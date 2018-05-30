using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using AutoMapper;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests.GetAuditHistoryHandlerTests.Given_Report_Is_Not_Submitted
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class Given_Report_Is_Not_Submitted
        : GivenWhenThen<GetAuditHistoryHandler>
    {
        private Mock<IReportRepository> mockRepository;
        private readonly Guid expectedReportId = new  Guid("3F0D018B-22DC-45B7-B81B-ED7C5CA024CF");

        protected override void Given()
        {
            SUT = new GetAuditHistoryHandler(
                SetupMockRepositoryReturn(),
                Mock.Of<IMapper>());
        }

        private IReportRepository SetupMockRepositoryReturn()
        {
            mockRepository = new Mock<IReportRepository>();

            mockRepository
                .Setup(
                    m => m.Get(
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                .Returns(
                    new ReportDto
                    {
                        Submitted = false,
                        Id = expectedReportId
                    });

            mockRepository
                .Setup(
                    m => m.Get(
                        It.IsAny<Guid>()))
                .Returns(
                    new ReportDto
                    {
                        Submitted = false,
                        Id = expectedReportId
                    });

            mockRepository
                .Setup(
                    m => m.GetAuditRecords(
                        It.IsAny<Guid>()))
                .Returns(
                    new List<AuditRecordDto>(0).AsReadOnly
                );

            return
                mockRepository
                    .Object;
        }

        protected override void When()
        {
            var requrestWithNonSubmittedReport =
                new GetAuditHistoryRequest(
                    period:new Period(DateTime.UtcNow), 
                    accountId:"SomeEmployerId");

            SUT.Handle(requrestWithNonSubmittedReport, new CancellationToken()).Wait();
        }

        [Test]
        public void Then_Repository_Is_Queried_For_History_Matching_Report_Id()
        {
            mockRepository
                .Verify(
                    m => m.GetAuditRecords( It.Is<Guid>( id => id.Equals(expectedReportId))));
        }
    }
}