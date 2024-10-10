using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Mapping;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;

namespace SFA.DAS.PSRService.Application.UnitTests.MappingTests;

[TestFixture]
public class Given_I_Want_To_Map_Report_Object
{
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ReportMappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Test]
    public void When_The_Mapping_Is_Registered_Then_Is_Valid()
    {
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Test]
    public void ReportCanSerialiseToAndFro()
    {
        var period = Period.FromInstantInPeriod(DateTime.UtcNow);
        var report = new Report
        {
            EmployerId = "11",
            OrganisationName = "12",
            HasMinimumEmployeeHeadcount = true,
            IsLocalAuthority = true,
            SerialNo = "1",
            Id = Guid.NewGuid(),
            ReportingPeriod = period.PeriodString,
            Period = period,
            Submitted = true,
            SubmittedDetails = new Submitted
            {
                SubmittedAt = DateTime.UtcNow,
                SubmittedEmail = "email",
                SubmittedName = "SN",
                SubmttedBy = "Dr Who",
                UniqueReference = "BBQ"
            },
            ReportingPercentages = new ReportingPercentages
            {
                EmploymentStarts = "11",
                TotalHeadCount = "22",
                NewThisPeriod = "33"
            },
            Sections =
            [
                new Section
                {
                    Id = "s1",
                    Title = "t1",
                    SummaryText = "s1s",
                    Questions =
                    [
                        new Question
                        {
                            Id = "s1q1",
                            Answer = "s1q1a",
                            Optional = true,
                            Type = QuestionType.LongText
                        },
                        new Question
                        {
                            Id = "s1q2",
                            Answer = "s1q2a",
                            Optional = false,
                            Type = QuestionType.ShortText
                        }
                    ],
                    SubSections = null
                },
                new Section
                {
                    Id = "s2",
                    Title = "t2",
                    SummaryText = "s2s",
                    Questions = null,
                    SubSections =
                    [
                        new Section
                        {
                            Id = "s2s1",
                            Title = "s2t1",
                            SummaryText = "s2s1s",
                            Questions =
                            [
                                new Question
                                {
                                    Id = "s2s1q1",
                                    Answer = "1",
                                    Optional = true,
                                    Type = QuestionType.Number
                                }
                            ]
                        }
                    ]
                }
            ]
        };

        // Act
        var dto = _mapper.Map<ReportDto>(report);
        var newReport = _mapper.Map<Report>(dto);

        // Assert
        report.EmployerId.Should().Be(newReport.EmployerId);
        report.OrganisationName.Should().Be(newReport.OrganisationName);
        report.HasMinimumEmployeeHeadcount.Should().Be(newReport.HasMinimumEmployeeHeadcount);
        report.IsLocalAuthority.Should().Be(newReport.IsLocalAuthority);
        report.SerialNo.Should().Be(newReport.SerialNo);
        report.Id.Should().Be(newReport.Id);
        report.ReportingPeriod.Should().Be(newReport.ReportingPeriod);
        report.Submitted.Should().Be(newReport.Submitted);

        report.SubmittedDetails.SubmittedAt.Should().Be(newReport.SubmittedDetails.SubmittedAt);
        report.SubmittedDetails.SubmittedEmail.Should().Be(newReport.SubmittedDetails.SubmittedEmail);
        report.SubmittedDetails.SubmittedName.Should().Be(newReport.SubmittedDetails.SubmittedName);
        report.SubmittedDetails.SubmttedBy.Should().Be(newReport.SubmittedDetails.SubmttedBy);
        report.SubmittedDetails.UniqueReference.Should().Be(newReport.SubmittedDetails.UniqueReference);

        report.ReportingPercentages.EmploymentStarts.Should().Be(newReport.ReportingPercentages.EmploymentStarts);
        report.ReportingPercentages.TotalHeadCount.Should().Be(newReport.ReportingPercentages.TotalHeadCount);
        report.ReportingPercentages.NewThisPeriod.Should().Be(newReport.ReportingPercentages.NewThisPeriod);

        report.Sections.Count().Should().Be(newReport.Sections.Count());
        var expectedSection = report.Sections.First();
        var actualSection = newReport.Sections.First();
        expectedSection.Id.Should().Be(actualSection.Id);
        expectedSection.SummaryText.Should().Be(actualSection.SummaryText);
        expectedSection.Title.Should().Be(actualSection.Title);
        expectedSection.Questions.Count().Should().Be(actualSection.Questions.Count());
    }

    [Test]
    public void When_I_Map_From_ReportDto_And_Json_is_Valid_Then_Return_Report()
    {
        var reportDto = new ReportDto
        {
            EmployerId = "ABCDE",
            ReportingData = "{\"OrganisationName\":\"Organisation 1\",\"HasMinimumEmployeeHeadcount\":true,\"IsLocalAuthority\":true,\"SerialNo\":\"2\",\"Questions\":\"\",\"Submitted\":null,ReportingPercentages:{EmploymentStarts: 11, NewThisPeriod: 22, TotalHeadCount: 33}}",
            ReportingPeriod = "1617",
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Submitted = true
        };

        var mappedReport = _mapper.Map<ReportDto, Report>(reportDto);

        mappedReport.ReportingPercentages.Should().NotBeNull();

        mappedReport.OrganisationName.Should().Be("Organisation 1");
        mappedReport.HasMinimumEmployeeHeadcount.Should().Be(true);
        mappedReport.IsLocalAuthority.Should().Be(true);
        mappedReport.SerialNo.Should().Be("2");

        mappedReport.Submitted.Should().Be(reportDto.Submitted);
        mappedReport.EmployerId.Should().Be(reportDto.EmployerId);
        mappedReport.Id.Should().Be(reportDto.Id);
        mappedReport.ReportingPeriod.Should().Be(reportDto.ReportingPeriod);
        mappedReport.ReportingPercentages.EmploymentStarts.Should().Be("11");
        mappedReport.ReportingPercentages.NewThisPeriod.Should().Be("22");
        mappedReport.ReportingPercentages.TotalHeadCount.Should().Be("33");
    }

    [Test]
    public void When_I_Map_From_Report_Then_Return_ReportDto()
    {
        var report = new Report
        {
            EmployerId = "ABCDE",
            OrganisationName = "Organisation 1",
            ReportingPeriod = "1617",
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Submitted = true,
            HasMinimumEmployeeHeadcount = true,
            IsLocalAuthority = true,
            SerialNo = "2",
            ReportingPercentages = new ReportingPercentages
            {
                EmploymentStarts = "11.00",
                TotalHeadCount = "22.00",
                NewThisPeriod = "33.00",
                Title = "ReportingPercentages"
            }
        };

        var mappedReportDto = _mapper.Map<Report, ReportDto>(report);

        const string expectedSerializedReportingData = "{\"OrganisationName\":\"Organisation 1\",\"HasMinimumEmployeeHeadcount\":true,\"IsLocalAuthority\":true,\"Questions\":null,\"SerialNo\":\"2\",\"Submitted\":null,\"ReportingPercentages\":{\"EmploymentStarts\":\"11.00\",\"TotalHeadCount\":\"22.00\",\"NewThisPeriod\":\"33.00\",\"Title\":\"ReportingPercentages\"},\"ReportingPercentagesSchools\":null}";

        expectedSerializedReportingData.Should().Be(mappedReportDto.ReportingData);
        report.Submitted.Should().Be(mappedReportDto.Submitted);
        report.EmployerId.Should().Be(mappedReportDto.EmployerId);
        report.Id.Should().Be(mappedReportDto.Id);
        report.ReportingPeriod.Should().Be(mappedReportDto.ReportingPeriod);
    }
}