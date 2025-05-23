﻿using System;
using System.Collections.Generic;
using System.Linq;
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

namespace SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests.GetSubmittedTests;

[TestFixture]
public class Given_I_Get_Submitted_Reports
{
    private Mock<IMapper> _mapperMock;
    private Mock<IReportRepository> _reportRepositoryMock;
    private IEnumerable<ReportDto> _reportDtoList;
    private IEnumerable<Report> _reportList;
    private GetSubmittedHandler _getSubmittedHandler;

    [SetUp]
    public void Setup()
    {
        _mapperMock = new Mock<IMapper>(MockBehavior.Strict);
        _reportRepositoryMock = new Mock<IReportRepository>(MockBehavior.Strict);
        _getSubmittedHandler = new GetSubmittedHandler(_reportRepositoryMock.Object, _mapperMock.Object);
        _mapperMock.Setup(s => s.Map<Report>(It.IsAny<ReportDto>())).Returns(new Report());

        _reportList = new List<Report>
        {
            new()
            {
                Id = Guid.NewGuid(),
                ReportingPeriod = "1718",
                EmployerId = "ABCDE",
                Submitted = false
            },
            new()
            {
                Id = Guid.NewGuid(),
                ReportingPeriod = "1617",
                EmployerId = "ABCDE",
                Submitted = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                ReportingPeriod = "1516",
                EmployerId = "ABCDE",
                Submitted = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                ReportingPeriod = "1718",
                EmployerId = "VWXYZ",
                Submitted = false
            }
        }.AsEnumerable();

        _reportDtoList = new List<ReportDto>
        {
            new()
            {
                Id = Guid.NewGuid(),
                ReportingPeriod = "1718",
                EmployerId = "ABCDE",
                Submitted = false
            },
            new()
            {
                Id = Guid.NewGuid(),
                ReportingPeriod = "1617",
                EmployerId = "ABCDE",
                Submitted = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                ReportingPeriod = "1516",
                EmployerId = "ABCDE",
                Submitted = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                ReportingPeriod = "1718",
                EmployerId = "ABCDE",
                Submitted = false
            }
        }.AsEnumerable();
    }

    [Test]
    public async Task When_An_Employer_Id_Is_Not_Supplied_Then_Return_Empty_Collection()
    {
        //arrange
        _reportRepositoryMock.Setup(s => s.GetSubmitted(It.IsAny<string>())).ReturnsAsync((List<ReportDto>)null);

        var getSubmittedRequest = new GetSubmittedRequest { EmployerId = string.Empty };

        //Act
        var result = await _getSubmittedHandler.Handle(getSubmittedRequest, new CancellationToken());

        //Assert
        result.Should().BeEmpty();
    }

    [Test]
    public async Task When_An_Employer_Id_Has_Submitted_Reports_Then_Return_List()
    {
        //arrange
        _reportRepositoryMock.Setup(s => s.GetSubmitted(It.IsAny<string>())).ReturnsAsync(_reportDtoList.Where(w => w.Submitted && w.EmployerId == "ABCDE").ToList);
        var getSubmittedRequest = new GetSubmittedRequest { EmployerId = "ABCDE" };

        //Act
        var result = await _getSubmittedHandler.Handle(getSubmittedRequest, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        var reports = result.ToList();
        reports.Count.Should().Be(2);
    }

    [Test]
    public async Task When_An_Employer_Id_Has_No_Submitted_Reports_Then_Return_Empty_Collection()
    {
        //arrange
        _reportRepositoryMock.Setup(s => s.GetSubmitted(It.IsAny<string>())).ReturnsAsync(new List<ReportDto>(0));

        var getSubmittedRequest = new GetSubmittedRequest { EmployerId = "knfjkdngkfngk" };

        //Act
        var result = await _getSubmittedHandler.Handle(getSubmittedRequest, new CancellationToken());

        //Assert
        result.Should().BeEmpty();
    }
}