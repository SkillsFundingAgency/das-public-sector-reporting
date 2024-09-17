using System;
using System.Threading;
using System.Threading.Tasks;
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
public class Given_I_Create_A_Report
{
    private Mock<IMapper> _mapperMock;
    private Mock<IReportRepository> _reportRepositoryMock;
    private Mock<IFileProvider> _fileProviderMock;
    private CreateReportHandler _createReportHandler;
    private Report _report;
    private const string EmployerId = "ABCDE";
    private const string ReportingPeriod = "1718";

    [SetUp]
    public void Setup()
    {
        _mapperMock = new Mock<IMapper>(MockBehavior.Strict);
        _reportRepositoryMock = new Mock<IReportRepository>(MockBehavior.Strict);
        _fileProviderMock = new Mock<IFileProvider>();
        _createReportHandler = new CreateReportHandler(_reportRepositoryMock.Object, _mapperMock.Object, _fileProviderMock.Object);

        _report = new Report
        {
            EmployerId = EmployerId,
            ReportingPeriod = ReportingPeriod,
            Id = Guid.NewGuid(),
            Submitted = false
        };

        var fileInfo = new StringFileInfo("", "QuestionConfig.json");

        _fileProviderMock.Setup(s => s.GetFileInfo(It.IsAny<string>())).Returns(fileInfo);
        _mapperMock.Setup(s => s.Map<Report>(It.IsAny<ReportDto>())).Returns(_report);
    }

    [Test]
    public async Task And_An_EmployeeId_And_Period_And_IsLocalAuthority_Is_Supplied_Then_Create_Report()
    {
        // arrange
        var userId = Guid.NewGuid();
        const string userName = "Bob Shurunkle";
        ReportDto reportDto = null;

        var createReportRequest =
            new CreateReportRequestBuilder()
                .WithUserName(userName)
                .WithUserId(userId)
                .WithEmployerId(EmployerId)
                .ForPeriod(ReportingPeriod)
                .WithIsLocalAuthority(false)
                .Build();

        _reportRepositoryMock.Setup(s => s.Create(It.IsAny<ReportDto>())).Verifiable();

        // act
        var result = await _createReportHandler.Handle(createReportRequest, new CancellationToken());

        // assert
        _reportRepositoryMock.VerifyAll();
        _report.EmployerId.Should().Be(result.EmployerId);
        _report.Submitted.Should().Be(result.Submitted);
        _report.ReportingPeriod.Should().Be(result.ReportingPeriod);
        _report.EmployerId.Should().Be(reportDto.EmployerId);
        _report.ReportingPeriod.Should().Be(reportDto.ReportingPeriod);

        reportDto.UpdatedBy.Contains(userId.ToString()).Should().BeTrue();
        reportDto.AuditWindowStartUtc.Should().NotBeNull();
        reportDto.UpdatedUtc.Should().NotBeNull();
    }

    [Test]
    public async Task And_An_EmployeeId_And_Period_And_As_IsLocalAuthority_Is_Supplied_Then_Create_Report()
    {
        // arrange
        var userId = Guid.NewGuid();
        const string userName = "Bob Shurunkle";
        ReportDto reportDto = null;

        var createReportRequest =
            new CreateReportRequestBuilder()
                .WithUserName(userName)
                .WithUserId(userId)
                .WithEmployerId(EmployerId)
                .ForPeriod(ReportingPeriod)
                .WithIsLocalAuthority(true)
                .Build();

        _reportRepositoryMock.Setup(s => s.Create(It.IsAny<ReportDto>())).Verifiable();

        // act
        var result = await _createReportHandler.Handle(createReportRequest, new CancellationToken());

        // assert
        _reportRepositoryMock.VerifyAll();
        _report.EmployerId.Should().Be(result.EmployerId);
        _report.Submitted.Should().Be(result.Submitted);
        _report.ReportingPeriod.Should().Be(result.ReportingPeriod);
        _report.EmployerId.Should().Be(reportDto.EmployerId);
        _report.ReportingPeriod.Should().Be(reportDto.ReportingPeriod);
        reportDto.UpdatedBy.Contains(userId.ToString()).Should().BeTrue();
        reportDto.AuditWindowStartUtc.Should().NotBeNull();
        reportDto.UpdatedUtc.Should().NotBeNull();
    }
}