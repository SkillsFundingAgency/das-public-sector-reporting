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
    public class Given_I_Want_To_Update_A_Report
    {
        private Mock<IMapper> _mapperMock;
        private Mock<IReportRepository> _reportRepositoryMock;
        private Mock<IFileProvider> _fileProviderMock;
        private UpdateReportHandler _updateReportHandler;
        private ReportDto reportDto;
        private Report _report;

        [SetUp]
        public void Setup()
        {
            _mapperMock = new Mock<IMapper>(MockBehavior.Strict);
            _reportRepositoryMock = new Mock<IReportRepository>(MockBehavior.Strict);
            _fileProviderMock = new Mock<IFileProvider>();
            _updateReportHandler = new UpdateReportHandler(_mapperMock.Object, _reportRepositoryMock.Object);

            var reportingDataTest = "";
            reportDto = new ReportDto()
            {
                EmployerId = "1234",
                ReportingPeriod = "1718",
                Id = Guid.NewGuid(),
                Submitted = false,
                ReportingData = reportingDataTest
            };

            _report = new Report()
            {
                EmployerId = "1234",
                ReportingPeriod = "1718",
                Id = Guid.NewGuid(),
                Submitted = false
            };
            var fileInfo = new StringFileInfo(reportingDataTest, "QuestionConfig.json");

            _reportRepositoryMock.Setup(s => s.Update(It.IsAny<ReportDto>()));
            _fileProviderMock.Setup(s => s.GetFileInfo(It.IsAny<string>())).Returns(fileInfo);
            _mapperMock.Setup(s => s.Map<Report>(It.IsAny<ReportDto>())).Returns(_report);
            _mapperMock.Setup(s => s.Map<ReportDto>(It.IsAny<Report>())).Returns(reportDto);
        }

        [Test]
        public void And_A_Report_Is_Supplied_Then_Create_Report()
        {
            //arrange

            var updateReportRequest = new UpdateReportRequest(new Report());

            //Act
            var result = _updateReportHandler.Handle(updateReportRequest, new CancellationToken());

            _reportRepositoryMock.Verify(s => s.Update(reportDto));
        }
    }
}




   