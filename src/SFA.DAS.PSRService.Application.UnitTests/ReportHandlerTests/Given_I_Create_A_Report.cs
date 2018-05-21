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

        [SetUp]
        public void Setup()
        {
            _mapperMock = new Mock<IMapper>(MockBehavior.Strict);
            _reportRepositoryMock = new Mock<IReportRepository>(MockBehavior.Strict);
            _fileProviderMock = new Mock<IFileProvider>();
            _createReportHandler = new CreateReportHandler(_reportRepositoryMock.Object, _mapperMock.Object, _fileProviderMock.Object);

            _report = new Report
            {
                EmployerId = "ABCDE",
                ReportingPeriod = "1718",
                Id = Guid.NewGuid(),
                Submitted = false
            };
            var fileInfo = new StringFileInfo("", "QuestionConfig.json");

            _fileProviderMock.Setup(s => s.GetFileInfo(It.IsAny<string>())).Returns(fileInfo);
            _mapperMock.Setup(s => s.Map<Report>(It.IsAny<ReportDto>())).Returns(_report);

        }

        [Test]
        public void And_An_EmployeeId_And_Period_Is_Supplied_Then_Create_Report()
        {
            // arrange
            var userId = Guid.NewGuid();
            var userName = "Bob Shurunkle";
            ReportDto reportDto = null;

            var createReportRequest = new CreateReportRequest { EmployerId = "ABCDE", Period = "1718", UserName = userName, UserId = userId};
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
        public void And_An_EmployeeId_Is_Not_Supplied_Then_Throw_Error()
        {
            //arrange
            var createReportRequest = new CreateReportRequest { EmployerId = string.Empty, Period = "1718", UserName = "Homer"};

            //Act
            Assert.Throws<Exception>(() =>_createReportHandler.Handle(createReportRequest, new CancellationToken()));
        }

        [Test]
        public void And_An_User_Is_Not_Supplied_Then_Throw_Error()
        {
            // arrange            
            var createReportRequest = new CreateReportRequest {EmployerId = "acme inc", Period = "1718"};

            // act
            // assert
            Assert.Throws<Exception>(() => _createReportHandler.Handle(createReportRequest, new CancellationToken()));
        }

        [Test]
        public void And_Period_Is_Null_Then_Throw_Error()
        {
            //arrange
            var createReportRequest = new CreateReportRequest { EmployerId = "ABCDE", Period = null, UserName = "Donald"};

            // act
            // assert
            Assert.Throws<Exception>(() => _createReportHandler.Handle(createReportRequest, new CancellationToken()));
        }

        [Test]
        public void And_Period_Is_Empty_Then_Throw_Error()
        {
            //arrange
            var createReportRequest = new CreateReportRequest() { EmployerId = "ABCDEF", Period = "", UserName = "Donald" };

            //Act
            Assert.Throws<Exception>(() => _createReportHandler.Handle(createReportRequest, new CancellationToken()));
        }
    }
}




   