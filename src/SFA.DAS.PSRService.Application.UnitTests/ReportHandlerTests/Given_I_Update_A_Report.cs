using System;
using System.Threading;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.FileProviders;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Application.UnitTests.FileInfo;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests;

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
        _updateReportHandler = new UpdateReportHandler(_mapperMock.Object, _reportRepositoryMock.Object, _fileProviderMock.Object);

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
        var updateUser = new User { Id = Guid.NewGuid(), Name = "Homer" };
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
                        UpdatedBy = new User { Id = Guid.Empty, Name = "Homer" }
                    })
                .Build();

        // act
        _updateReportHandler.Handle(updateReportRequest, default);

        // assert
        _reportRepositoryMock.VerifyAll();
        actualReportDto.Should().NotBeNull();
        justNow.Should().Be(actualReportDto.AuditWindowStartUtc);
        justNow.Should().NotBe(actualReportDto.UpdatedUtc);
        actualReportDto.UpdatedBy.Should().NotBeNull();
        actualReportDto.UpdatedBy.Should().Contain(updateUser.Name);
    }

    [Test]
    public void When_Report_Is_Updated_More_Than_Audit_Window_Size_Ago_Then_Audit_Record_Is_Created()
    {
        // arrange
        var longAgo = DateTime.UtcNow.AddMinutes(-6);
        ReportDto actualReportDto = null;
        AuditRecordDto actualAuditRecordDto = null;
        var reportId = Guid.NewGuid();
        var updateUser = new User { Id = Guid.NewGuid(), Name = "Homer" };

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
                        UpdatedBy = new User { Id = Guid.Empty, Name = "Homer" }
                    })
                .Build();

        // act
        _updateReportHandler.Handle(updateReportRequest, default);

        // assert
        _reportRepositoryMock.VerifyAll();
        actualReportDto.Should().NotBeNull();
        longAgo.Should().NotBe(actualReportDto.AuditWindowStartUtc);
        longAgo.Should().NotBe(actualReportDto.UpdatedUtc);
        actualReportDto.UpdatedBy.Should().NotBeNull();
        actualReportDto.UpdatedBy.Should().Contain(updateUser.Name);
        newVersion.ReportingData.Should().Be(actualReportDto.ReportingData);

        actualAuditRecordDto.Should().NotBeNull();
        reportId.Should().Be(actualAuditRecordDto.ReportId);
        longAgo.Should().Be(actualAuditRecordDto.UpdatedUtc);
        actualAuditRecordDto.UpdatedBy.Should().Contain(updateUser.Name);
        oldVersion.ReportingData.Should().Be(actualAuditRecordDto.ReportingData);
    }

    [Test]
    public void When_Report_Is_Updated_Less_Than_Audit_Window_Size_Ago_But_By_Different_User_Then_Audit_Record_Is_Created()
    {
        // arrange
        var justNow = DateTime.UtcNow;
        ReportDto actualReportDto = null;
        AuditRecordDto actualAuditRecordDto = null;
        var reportId = Guid.NewGuid();
        var updateUser = new User { Id = Guid.NewGuid(), Name = "Homer" };
        var oldUser = new User { Id = Guid.NewGuid(), Name = "Bob Shurunkle" };

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
                        UpdatedBy = new User { Id = Guid.Empty, Name = "Homer" }
                    })
                .Build();

        // act
        _updateReportHandler.Handle(updateReportRequest, default);

        // assert
        _reportRepositoryMock.VerifyAll();

        actualReportDto.Should().NotBeNull();
        justNow.Should().NotBe(actualReportDto.AuditWindowStartUtc);
        justNow.Should().NotBe(actualReportDto.UpdatedUtc);
        actualReportDto.UpdatedBy.Should().NotBeNull();
        actualReportDto.UpdatedBy.Should().Contain(updateUser.Name);
        newVersion.ReportingData.Should().Be(actualReportDto.ReportingData);

        actualAuditRecordDto.Should().NotBeNull();
        reportId.Should().Be(actualAuditRecordDto.ReportId);
        justNow.Should().Be(actualAuditRecordDto.UpdatedUtc);
        actualAuditRecordDto.UpdatedBy.Should().Contain(oldUser.Name);
        oldVersion.ReportingData.Should().Be(actualAuditRecordDto.ReportingData);
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
                    new Report { Id = reportId })
                .Build();

        _reportRepositoryMock.Setup(s => s.Get(reportId)).Returns((ReportDto)null).Verifiable();

        // act
        var action = () => _updateReportHandler.Handle(updateReportRequest, new CancellationToken());

        // assert
        action.Should().ThrowAsync<ApplicationException>();
    }

    [Test]
    public void When_Old_Version_Does_Not_Have_New_Values_They_Are_Recorded_And_No_Audit_Created()
    {
        // arrange
        ReportDto actualReportDto = null;
        var reportId = Guid.NewGuid();
        var updateUser = new User { Id = Guid.NewGuid(), Name = "Homer" };

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
        _updateReportHandler.Handle(updateReportRequest, default);

        // assert
        _reportRepositoryMock.VerifyAll();

        actualReportDto.Should().NotBeNull();
        actualReportDto.AuditWindowStartUtc.Should().NotBeNull();
        actualReportDto.UpdatedUtc.Should().NotBeNull();
        actualReportDto.UpdatedBy.Should().NotBeNull();
        actualReportDto.UpdatedBy.Should().Contain(updateUser.Name);
        newVersion.ReportingData.Should().Be(actualReportDto.ReportingData);
    }
}