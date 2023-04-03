using System;
using System.Threading;
using AutoMapper;
using Microsoft.Extensions.FileProviders;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Application.UnitTests.FileInfo;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests
{
    [TestFixture]
    public class Given_I_Create_A_Report
    {
        private Mock<IMapper> _mapperMock;
        private Mock<IReportRepository> _reportRepositoryMock;
        private Mock<IFileProvider> _fileProviderMock;
        private CreateReportHandler _createReportHandler;
        private Report _report;
        private string _employerId = "ABCDE";
        private string _reportingPeriod = "1718";

        [SetUp]
        public void Setup()
        {
            _mapperMock = new Mock<IMapper>(MockBehavior.Strict);
            _reportRepositoryMock = new Mock<IReportRepository>(MockBehavior.Strict);
            _fileProviderMock = new Mock<IFileProvider>();
            _createReportHandler = new CreateReportHandler(_reportRepositoryMock.Object, _mapperMock.Object, _fileProviderMock.Object);

            _report = new Report
            {
                EmployerId = _employerId,
                ReportingPeriod = _reportingPeriod,
                Id = Guid.NewGuid(),
                Submitted = false
            };
            var fileInfo = new StringFileInfo("", "QuestionConfig.json");

            _fileProviderMock.Setup(s => s.GetFileInfo(It.IsAny<string>())).Returns(fileInfo);
            _mapperMock.Setup(s => s.Map<Report>(It.IsAny<ReportDto>())).Returns(_report);
        }

        [Test]
        public void And_An_EmployeeId_And_Period_And_IsLocalAuthority_Is_Supplied_Then_Create_Report()
        {
            // arrange
            var userId = Guid.NewGuid();
            var userName = "Bob Shurunkle";
            ReportDto reportDto = null;

            var createReportRequest =
                new CreateReportRequestBuilder()
                    .WithUserName(userName)
                    .WithUserId(userId)
                    .WithEmployerId(_employerId)
                    .ForPeriod(_reportingPeriod)
                    .WithIsLocalAuthority(false)
                    .Build();

            _reportRepositoryMock.Setup(s => s.Create(It.IsAny<ReportDto>())).Callback<ReportDto>(r => reportDto = r).Verifiable();

            // act
            var result = _createReportHandler.Handle(createReportRequest, new CancellationToken()).Result;

            // assert
            _reportRepositoryMock.VerifyAll();
            Assert.AreEqual(_report.EmployerId, result.EmployerId);
            Assert.AreEqual(_report.Submitted, result.Submitted);
            Assert.AreEqual(_report.ReportingPeriod, result.ReportingPeriod);
            Assert.AreEqual(_report.EmployerId, reportDto.EmployerId);
            Assert.AreEqual(_report.ReportingPeriod, reportDto.ReportingPeriod);
            Assert.IsTrue(reportDto.UpdatedBy.Contains(userId.ToString()));
            Assert.IsNotNull(reportDto.AuditWindowStartUtc);
            Assert.IsNotNull(reportDto.UpdatedUtc);
        }

        [Test]
        public void And_An_EmployeeId_And_Period_And_As_IsLocalAuthority_Is_Supplied_Then_Create_Report()
        {
            // arrange
            var userId = Guid.NewGuid();
            var userName = "Bob Shurunkle";
            ReportDto reportDto = null;

            var createReportRequest =
                new CreateReportRequestBuilder()
                    .WithUserName(userName)
                    .WithUserId(userId)
                    .WithEmployerId(_employerId)
                    .ForPeriod(_reportingPeriod)
                    .WithIsLocalAuthority(true)
                    .Build();

            _reportRepositoryMock.Setup(s => s.Create(It.IsAny<ReportDto>())).Callback<ReportDto>(r => reportDto = r).Verifiable();

            // act
            var result = _createReportHandler.Handle(createReportRequest, new CancellationToken()).Result;

            // assert
            _reportRepositoryMock.VerifyAll();
            Assert.AreEqual(_report.EmployerId, result.EmployerId);
            Assert.AreEqual(_report.Submitted, result.Submitted);
            Assert.AreEqual(_report.ReportingPeriod, result.ReportingPeriod);
            Assert.AreEqual(_report.EmployerId, reportDto.EmployerId);
            Assert.AreEqual(_report.ReportingPeriod, reportDto.ReportingPeriod);
            Assert.IsTrue(reportDto.UpdatedBy.Contains(userId.ToString()));
            Assert.IsNotNull(reportDto.AuditWindowStartUtc);
            Assert.IsNotNull(reportDto.UpdatedUtc);
        }
    }
}




   