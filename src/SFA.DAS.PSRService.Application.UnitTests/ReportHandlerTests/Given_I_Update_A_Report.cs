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
    public class Given_I_Update_A_Report
    {
        private Mock<IMapper> _mapperMock;
        private Mock<IReportRepository> _reportRepositoryMock;
        private Mock<IFileProvider> _fileProviderMock;
        private UpdateReportHandler _updateReportHandler;

        [SetUp]
        public void Setup()
        {
            _mapperMock = new Mock<IMapper>(MockBehavior.Strict);
            _reportRepositoryMock = new Mock<IReportRepository>(MockBehavior.Strict);
            _fileProviderMock = new Mock<IFileProvider>();
            _updateReportHandler = new UpdateReportHandler( _mapperMock.Object, _reportRepositoryMock.Object);

            var fileInfo = new StringFileInfo("", "QuestionConfig.json");
            _fileProviderMock.Setup(s => s.GetFileInfo(It.IsAny<string>())).Returns(fileInfo);
        }

        [Test]
        public void When_Report_Is_Updated_Less_Than_Audit_Window_Size_Ago_Then_Audit_Record_Is_Not_Created()
        {
            // arrange
            var justNow = DateTime.UtcNow;
            ReportDto actualReportDto = null;
            var reportId = Guid.NewGuid();
            var updateUser = new User {Id = Guid.NewGuid(), Name = "Homer"};
            var oldVersion = new ReportDto
            {
                Id = reportId,
                AuditWindowStartUtc = justNow,
                UpdatedUtc = justNow,
                UpdatedBy = $"{{ Id: '{updateUser.Id}', Name: '{updateUser.Name}' }}"
            };
            var newVersion = new ReportDto
            {
                Id = reportId,
                AuditWindowStartUtc = justNow,
                UpdatedUtc = justNow,
                UpdatedBy = $"{{ Id: '{updateUser.Id}', Name: '{updateUser.Name}' }}"
            };

            _reportRepositoryMock.Setup(s => s.Get(reportId)).Returns(oldVersion).Verifiable();
            _reportRepositoryMock.Setup(s => s.Update(It.IsAny<ReportDto>())).Callback<ReportDto>(d => actualReportDto = d).Verifiable();
            _mapperMock.Setup(s => s.Map<ReportDto>(It.IsAny<Report>())).Returns(newVersion);

            var updateReportRequest =
                new UpdateReportRequestBuilder()
                    .WithUserName(updateUser.Name)
                    .WithUserId(updateUser.Id)
                    .WithAutoWindowSizeInMinutes(5)
                    .WithReport(
                        new Report
                        {
                            Id = reportId,
                            AuditWindowStartUtc = justNow,
                            UpdatedUtc = justNow,
                            UpdatedBy = new User {Id = Guid.Empty, Name = "Homer"}
                        })
                    .Build();

            // act
            _updateReportHandler.Handle(updateReportRequest, default(CancellationToken));

            // assert
            _reportRepositoryMock.VerifyAll();
            Assert.IsNotNull(actualReportDto);
            Assert.AreEqual(justNow, actualReportDto.AuditWindowStartUtc);
            Assert.AreNotEqual(justNow, actualReportDto.UpdatedUtc);
            Assert.IsNotNull(actualReportDto.UpdatedBy);
            Assert.IsTrue(actualReportDto.UpdatedBy.Contains(updateUser.Name));
        }

        [Test]
        public void When_Report_Is_Updated_More_Than_Audit_Window_Size_Ago_Then_Audit_Record_Is_Created()
        {
            // arrange
            var longAgo = DateTime.UtcNow.AddMinutes(-6);
            ReportDto actualReportDto = null;
            AuditRecordDto actualAuditRecordDto = null;
            var reportId = Guid.NewGuid();
            var updateUser = new User {Id = Guid.NewGuid(), Name = "Homer"};

            var oldVersion = new ReportDto
            {
                Id = reportId,
                AuditWindowStartUtc = longAgo,
                UpdatedUtc = longAgo,
                UpdatedBy = $"{{ Id: '{updateUser.Id}', Name: '{updateUser.Name}' }}",
                ReportingData = "old report"
            };

            var newVersion = new ReportDto
            {
                Id = reportId,
                AuditWindowStartUtc = longAgo,
                UpdatedUtc = longAgo,
                UpdatedBy = $"{{ Id: '{updateUser.Id}', Name: '{updateUser.Name}' }}",
                ReportingData = "new report"
            };

            _reportRepositoryMock.Setup(s => s.Get(reportId)).Returns(oldVersion).Verifiable();
            _reportRepositoryMock.Setup(s => s.SaveAuditRecord(It.IsAny<AuditRecordDto>()))
                .Callback<AuditRecordDto>(d => actualAuditRecordDto = d).Verifiable();
            _reportRepositoryMock.Setup(s => s.Update(It.IsAny<ReportDto>()))
                .Callback<ReportDto>(d => actualReportDto = d).Verifiable();
            _mapperMock.Setup(s => s.Map<ReportDto>(It.IsAny<Report>())).Returns(newVersion);

            var updateReportRequest =
                new UpdateReportRequestBuilder()
                    .WithUserName(updateUser.Name)
                    .WithUserId(updateUser.Id)
                    .WithAutoWindowSizeInMinutes(5)
                    .WithReport(
                        new Report
                        {
                            Id = reportId,
                            AuditWindowStartUtc = longAgo,
                            UpdatedUtc = longAgo,
                            UpdatedBy = new User {Id = Guid.Empty, Name = "Homer"}
                        })
                    .Build();

            // act
            _updateReportHandler.Handle(updateReportRequest, default(CancellationToken));

            // assert
            _reportRepositoryMock.VerifyAll();
            Assert.IsNotNull(actualReportDto);
            Assert.AreNotEqual(longAgo, actualReportDto.AuditWindowStartUtc);
            Assert.AreNotEqual(longAgo, actualReportDto.UpdatedUtc);
            Assert.IsNotNull(actualReportDto.UpdatedBy);
            Assert.IsTrue(actualReportDto.UpdatedBy.Contains(updateUser.Name));
            Assert.AreEqual(newVersion.ReportingData, actualReportDto.ReportingData);

            Assert.IsNotNull(actualAuditRecordDto);
            Assert.AreEqual(reportId, actualAuditRecordDto.ReportId);
            Assert.AreEqual(longAgo, actualAuditRecordDto.UpdatedUtc);
            Assert.IsTrue(actualAuditRecordDto.UpdatedBy.Contains(updateUser.Name));
            Assert.AreEqual(oldVersion.ReportingData, actualAuditRecordDto.ReportingData);
        }

        [Test]
        public void When_Report_Is_Updated_Less_Than_Audit_Window_Size_Ago_But_By_Different_User_Then_Audit_Record_Is_Created()
        {
            // arrange
            var justNow = DateTime.UtcNow;
            ReportDto actualReportDto = null;
            AuditRecordDto actualAuditRecordDto = null;
            var reportId = Guid.NewGuid();
            var updateUser = new User {Id = Guid.NewGuid(), Name = "Homer"};
            var oldUser = new User {Id = Guid.NewGuid(), Name = "Bob Shurunkle"};

            var oldVersion = new ReportDto
            {
                Id = reportId,
                AuditWindowStartUtc = justNow,
                UpdatedUtc = justNow,
                UpdatedBy = $"{{ Id: '{oldUser.Id}', Name: '{oldUser.Name}' }}",
                ReportingData = "old report"
            };

            var newVersion = new ReportDto
            {
                Id = reportId,
                AuditWindowStartUtc = justNow,
                UpdatedUtc = justNow,
                UpdatedBy = $"{{ Id: '{updateUser.Id}', Name: '{updateUser.Name}' }}",
                ReportingData = "new report"
            };

            _reportRepositoryMock.Setup(s => s.Get(reportId)).Returns(oldVersion).Verifiable();
            _reportRepositoryMock.Setup(s => s.SaveAuditRecord(It.IsAny<AuditRecordDto>())).Callback<AuditRecordDto>(d => actualAuditRecordDto = d).Verifiable();
            _reportRepositoryMock.Setup(s => s.Update(It.IsAny<ReportDto>())).Callback<ReportDto>(d => actualReportDto = d).Verifiable();
            _mapperMock.Setup(s => s.Map<ReportDto>(It.IsAny<Report>())).Returns(newVersion);

            var updateReportRequest =
                new UpdateReportRequestBuilder()
                    .WithUserName(updateUser.Name)
                    .WithUserId(updateUser.Id)
                    .WithAutoWindowSizeInMinutes(5)
                    .WithReport(
                        new Report
                        {
                            Id = reportId,
                            AuditWindowStartUtc = justNow,
                            UpdatedUtc = justNow,
                            UpdatedBy = new User {Id = Guid.Empty, Name = "Homer"}
                        })
                    .Build();

            // act
            _updateReportHandler.Handle(updateReportRequest, default(CancellationToken));

            // assert
            _reportRepositoryMock.VerifyAll();

            Assert.IsNotNull(actualReportDto);
            Assert.AreNotEqual(justNow, actualReportDto.AuditWindowStartUtc);
            Assert.AreNotEqual(justNow, actualReportDto.UpdatedUtc);
            Assert.IsNotNull(actualReportDto.UpdatedBy);
            Assert.IsTrue(actualReportDto.UpdatedBy.Contains(updateUser.Name));
            Assert.AreEqual(newVersion.ReportingData, actualReportDto.ReportingData);

            Assert.IsNotNull(actualAuditRecordDto);
            Assert.AreEqual(reportId, actualAuditRecordDto.ReportId);
            Assert.AreEqual(justNow, actualAuditRecordDto.UpdatedUtc);
            Assert.IsTrue(actualAuditRecordDto.UpdatedBy.Contains(oldUser.Name));
            Assert.AreEqual(oldVersion.ReportingData, actualAuditRecordDto.ReportingData);
        }

        [Test]
        public void When_Old_Version_Cannot_Be_Found_Then_Throw_Error()
        {
            // arrange
            var reportId = Guid.NewGuid();
            var updateReportRequest =
                new UpdateReportRequestBuilder()
                    .WithUserName("Bob Shurunkle")
                    .WithReport(
                        new Report {Id = reportId})
                    .Build();

            _reportRepositoryMock.Setup(s => s.Get(reportId)).Returns((ReportDto) null).Verifiable();

            // act
            // assert
            Assert.Throws<Exception>(() => _updateReportHandler.Handle(updateReportRequest, new CancellationToken()));
        }

        [Test]
        public void When_Old_Version_Doesnt_Have_New_Values_They_Are_Recorded_And_No_Audit_Created()
        {
            // arrange
            var justNow = DateTime.UtcNow.AddSeconds(-1);
            ReportDto actualReportDto = null;
            var reportId = Guid.NewGuid();
            var updateUser = new User {Id = Guid.NewGuid(), Name = "Homer"};

            var oldVersion = new ReportDto
            {
                Id = reportId,
                AuditWindowStartUtc = null,
                UpdatedUtc = null,
                UpdatedBy = null,
                ReportingData = "old report"
            };

            var newVersion = new ReportDto
            {
                Id = reportId,
                AuditWindowStartUtc = null,
                UpdatedUtc = null,
                UpdatedBy = null,
                ReportingData = "new report"
            };

            _reportRepositoryMock.Setup(s => s.Get(reportId)).Returns(oldVersion).Verifiable();
            _reportRepositoryMock.Setup(s => s.Update(It.IsAny<ReportDto>())).Callback<ReportDto>(d => actualReportDto = d).Verifiable();
            _mapperMock.Setup(s => s.Map<ReportDto>(It.IsAny<Report>())).Returns(newVersion);

            var updateReportRequest =
                new UpdateReportRequestBuilder()
                    .WithUserName(updateUser.Name)
                    .WithUserId(updateUser.Id)
                    .WithAutoWindowSizeInMinutes(5)
                    .WithReport(
                        new Report
                        {
                            Id = reportId,
                            AuditWindowStartUtc = null,
                            UpdatedUtc = null,
                            UpdatedBy = null
                        })
                    .Build();

            // act
            _updateReportHandler.Handle(updateReportRequest, default(CancellationToken));

            // assert
            _reportRepositoryMock.VerifyAll();

            Assert.IsNotNull(actualReportDto);
            Assert.IsNotNull(actualReportDto.AuditWindowStartUtc);
            Assert.IsNotNull(actualReportDto.UpdatedUtc);
            Assert.IsNotNull(actualReportDto.UpdatedBy);
            Assert.IsTrue(actualReportDto.UpdatedBy.Contains(updateUser.Name));
            Assert.AreEqual(newVersion.ReportingData, actualReportDto.ReportingData);
        }
    }
}   