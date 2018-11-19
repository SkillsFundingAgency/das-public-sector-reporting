using System;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Mapping;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.UnitTests.MappingTests
{
    [TestFixture]
    public class Given_I_Want_To_Map_Between_Report_And_ReportDto_Objects
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
            var report = ReportBuilder.BuildValidSubmittedReport();

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
                    EmploymentStarts = "11.00",
                    TotalHeadCount = "22.00",
                    NewThisPeriod = "33.00"
                }
            };

            var mappedReportDto = _mapper.Map<Report, ReportDto>(report);

            var mappedReportingData = JsonConvert.DeserializeObject<ReportingData>(mappedReportDto.ReportingData);

            Assert.AreEqual(report.OrganisationName, mappedReportingData.OrganisationName);
            Assert.AreEqual((report.Sections ?? Enumerable.Empty<Section>()), (mappedReportingData.Questions ?? Enumerable.Empty<Section>()));
            Assert.That(mappedReportingData.ReportingPercentages, Is.EqualTo(report.ReportingPercentages).Using(new ReportingPercentagesEqualityComparer()));
            Assert.That(mappedReportingData.Submitted, Is.EqualTo(report.SubmittedDetails).Using(new SubmittedEqualityComparer()));

            Assert.AreEqual(report.Submitted, mappedReportDto.Submitted );
            Assert.AreEqual(report.EmployerId, mappedReportDto.EmployerId);
            Assert.AreEqual(report.Id, mappedReportDto.Id);
            Assert.AreEqual(report.ReportingPeriod, mappedReportDto.ReportingPeriod);
        }

        private string serializeReportingDataFromReport(Report report)
        {
            return
                JsonConvert.SerializeObject(
                    new ReportingData
                    {
                        OrganisationName = report.OrganisationName,
                        Questions = report.Sections,
                        ReportingPercentages = report.ReportingPercentages,
                        Submitted = report.SubmittedDetails
                    }
                );
        }

        private void AssertSerializedReportingDataAreEquivalent(string leftSerialization, string rightSerialization)
        {
            var leftData = JsonConvert.DeserializeObject<ReportingData>(leftSerialization);
            var rightData = JsonConvert.DeserializeObject<ReportingData>(rightSerialization);

            AssertReportingDataAreEquivalent(leftData, rightData);
        }

        private void AssertReportingDataAreEquivalent(ReportingData leftData, ReportingData rightData)
        {
            Assert.AreEqual(leftData.OrganisationName, rightData.OrganisationName);

            Assert.AreEqual((leftData.Questions ?? Enumerable.Empty<Section>()), (rightData.Questions ?? Enumerable.Empty<Section>()));

            Assert.AreEqual(leftData.Submitted, rightData.Submitted);

            Assert.AreEqual(leftData.ReportingPercentages, rightData.ReportingPercentages);
        }
    }
}
