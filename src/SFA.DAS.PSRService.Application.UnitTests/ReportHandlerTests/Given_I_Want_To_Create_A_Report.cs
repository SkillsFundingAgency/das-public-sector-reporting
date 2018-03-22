using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private ReportDto reportDto;
        private Report _report;

        [SetUp]
        public void Setup()
        {
            _mapperMock = new Mock<IMapper>(MockBehavior.Strict);
            _reportRepositoryMock = new Mock<IReportRepository>(MockBehavior.Strict);
            _fileProviderMock = new Mock<IFileProvider>();
            _createReportHandler = new CreateReportHandler(_reportRepositoryMock.Object, _mapperMock.Object, _fileProviderMock.Object);

            var reportingDataTest = "";
            reportDto = new ReportDto()
            {
                EmployerId = 1234,
                ReportingPeriod = "1718",
                Id = Guid.NewGuid(),
                Submitted = false,
                ReportingData = reportingDataTest


            };

            _report = new Report()
            {
                EmployerId = 1234,
                ReportingPeriod = "1718",
                Id = Guid.NewGuid(),
                Submitted = false


            };
            var fileInfo = new StringFileInfo(reportingDataTest, "QuestionConfig.json");

            _reportRepositoryMock.Setup(s => s.Create(It.IsAny<ReportDto>())).Returns(reportDto);
            _fileProviderMock.Setup(s => s.GetFileInfo(It.IsAny<string>())).Returns(fileInfo);
            _mapperMock.Setup(s => s.Map<Report>(It.IsAny<ReportDto>())).Returns(_report);

        }
        [Test]
        public void And_An_EmployeeId_And_Period_Is_Supplied_Then_Create_Report()
        {

            //arrange

            var createReportRequest = new CreateReportRequest() { EmployerId = 12345, Period = "1718" };

            //Act
            var result = _createReportHandler.Handle(createReportRequest, new CancellationToken()).Result;

            Assert.AreEqual(_report.EmployerId, result.EmployerId);
            Assert.AreEqual(result.Submitted,_report.Submitted);
            Assert.AreEqual(result.ReportingPeriod,_report.ReportingPeriod);
            
        }
        [Test]
        public void And_An_EmployeeId_Is_Not_Supplied_Then_Throw_Error()
        {
            
            //arrange
            
            var createReportRequest = new CreateReportRequest() { EmployerId = 0, Period = "1718"};

            //Act

            Assert.Throws<Exception>(() =>_createReportHandler.Handle(createReportRequest, new CancellationToken()));

          


        }
        [Test]
        public void And_Period_Is_Null_Then_Throw_Error()
        {

            //arrange

            var createReportRequest = new CreateReportRequest() { EmployerId = 12345, Period = null };

            //Act

            Assert.Throws<Exception>(() => _createReportHandler.Handle(createReportRequest, new CancellationToken()));

            //Assert
            


        }

        [Test]
        public void And_Period_Is_Empty_Then_Throw_Error()
        {

            //arrange

            var createReportRequest = new CreateReportRequest() { EmployerId = 123450, Period = "" };

            //Act

            Assert.Throws<Exception>(() => _createReportHandler.Handle(createReportRequest, new CancellationToken()));




        }
    }
}




   