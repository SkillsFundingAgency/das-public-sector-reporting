using System;
using System.Linq;
using AutoMapper;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Mapping;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;

namespace SFA.DAS.PSRService.Application.UnitTests.MappingTests
{
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
            var period = new Period(DateTime.UtcNow);
            var report = new Report
            {
                EmployerId = "11",
                OrganisationName = "12",
                Id = Guid.NewGuid(),
                ReportingPeriod = period.PeriodString,
                Period = period ,
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
                Sections = new[]
                {
                    new Section
                    {
                        Id = "s1",
                        Title = "t1",
                        SummaryText = "s1s",
                        Questions = new[]
                        {
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
                        },
                        SubSections = null
                    },
                    new Section
                    {
                        Id = "s2",
                        Title = "t2",
                        SummaryText = "s2s",
                        Questions = null,
                        SubSections = new[]
                        {
                            new Section
                            {
                                Id = "s2s1",
                                Title = "s2t1",
                                SummaryText = "s2s1s",
                                Questions = new[]
                                {
                                    new Question
                                    {
                                        Id = "s2s1q1",
                                        Answer = "1",
                                        Optional = true,
                                        Type = QuestionType.Number
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act
            var dto = _mapper.Map<ReportDto>(report);
            var newReport = _mapper.Map<Report>(dto);

            // Assert
            Assert.AreEqual(report.EmployerId, newReport.EmployerId);
            Assert.AreEqual(report.OrganisationName, newReport.OrganisationName);
            Assert.AreEqual(report.Id, newReport.Id);
            Assert.AreEqual(report.ReportingPeriod, newReport.ReportingPeriod);
            Assert.AreEqual(report.Submitted, newReport.Submitted);

            Assert.AreEqual(report.SubmittedDetails.SubmittedAt, newReport.SubmittedDetails.SubmittedAt);
            Assert.AreEqual(report.SubmittedDetails.SubmittedEmail, newReport.SubmittedDetails.SubmittedEmail);
            Assert.AreEqual(report.SubmittedDetails.SubmittedName, newReport.SubmittedDetails.SubmittedName);
            Assert.AreEqual(report.SubmittedDetails.SubmttedBy, newReport.SubmittedDetails.SubmttedBy);
            Assert.AreEqual(report.SubmittedDetails.UniqueReference, newReport.SubmittedDetails.UniqueReference);

            Assert.AreEqual(report.ReportingPercentages.EmploymentStarts, newReport.ReportingPercentages.EmploymentStarts);
            Assert.AreEqual(report.ReportingPercentages.TotalHeadCount, newReport.ReportingPercentages.TotalHeadCount);
            Assert.AreEqual(report.ReportingPercentages.NewThisPeriod, newReport.ReportingPercentages.NewThisPeriod);

            Assert.AreEqual(report.Sections.Count(), newReport.Sections.Count());
            var expectedSection = report.Sections.First();
            var actualSection = newReport.Sections.First();
            Assert.AreEqual(expectedSection.Id, actualSection.Id);
            Assert.AreEqual(expectedSection.SummaryText, actualSection.SummaryText);
            Assert.AreEqual(expectedSection.Title, actualSection.Title);
            Assert.AreEqual(expectedSection.Questions.Count(), actualSection.Questions.Count());
        }

        [Test]
        public void When_I_Map_From_ReportDto_And_Json_is_Valid_Then_Return_Report()
        {
            //Mapper.AssertConfigurationIsValid();

            var reportDto = new ReportDto()
            {
                EmployerId = "ABCDE",
                ReportingData = "{\"OrganisationName\":\"Organisation 1\",\"Questions\":\"\",\"Submitted\":null,ReportingPercentages:{EmploymentStarts: 11, NewThisPeriod: 22, TotalHeadCount: 33}}",
                ReportingPeriod = "1617",
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Submitted = true
            };

            var mappedReport = _mapper.Map<ReportDto, Report>(reportDto);

            Assert.AreEqual(mappedReport.OrganisationName, "Organisation 1");
            Assert.AreEqual(mappedReport.Submitted, reportDto.Submitted);
            Assert.AreEqual(mappedReport.EmployerId, reportDto.EmployerId);
            Assert.AreEqual(mappedReport.Id, reportDto.Id);
            Assert.AreEqual(mappedReport.ReportingPeriod, reportDto.ReportingPeriod);
            Assert.IsNotNull(mappedReport.ReportingPercentages);
            Assert.AreEqual("11", mappedReport.ReportingPercentages.EmploymentStarts);
            Assert.AreEqual("22", mappedReport.ReportingPercentages.NewThisPeriod);
            Assert.AreEqual("33", mappedReport.ReportingPercentages.TotalHeadCount);
        }


        [Test]
        public void When_I_Map_From_Report_Then_Return_ReportDto()
        {
            //Mapper.AssertConfigurationIsValid();

            var report = new Report()
            {
                EmployerId = "ABCDE",
                OrganisationName = "Organisation 1",
                ReportingPeriod = "1617",
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Submitted = true,
                ReportingPercentages = new ReportingPercentages
                {
                    EmploymentStarts = "11",
                    TotalHeadCount = "22",
                    NewThisPeriod = "33"
                }
            };

            var mappedReportDto = _mapper.Map<Report, ReportDto>(report);

            Assert.AreEqual(mappedReportDto.ReportingData, "{\"OrganisationName\":\"Organisation 1\",\"Questions\":null,\"Submitted\":null,\"ReportingPercentages\":{\"EmploymentStarts\":11.0,\"TotalHeadCount\":22.0,\"NewThisPeriod\":33.0}}");
            Assert.AreEqual(mappedReportDto.Submitted, report.Submitted);
            Assert.AreEqual(mappedReportDto.EmployerId, report.EmployerId);
            Assert.AreEqual(mappedReportDto.Id, report.Id);
            Assert.AreEqual(mappedReportDto.ReportingPeriod, report.ReportingPeriod);
        }
    }
}
