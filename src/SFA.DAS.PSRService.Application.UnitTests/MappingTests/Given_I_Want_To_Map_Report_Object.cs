using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AutoMapper;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Application.Mapping;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests.GetSubmittedTests
{
    [TestFixture]
    public class Given_I_Want_To_Map_Report_Object
    {
        private IMapper _mapper;

        private IEnumerable<ReportDto> _reportDtoList;
        private IEnumerable<Report> _reportList;

        private GetSubmittedHandler _getSubmittedHandler;

        [SetUp]
        public void Setup()
        {
            

            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<ReportMappingProfile>();
               
            });
            
            _mapper = config.CreateMapper();

            _reportList = (new List<Report>()
            {
                new Report()
                {
                    Id = Guid.NewGuid(),
                    ReportingPeriod = "1718",
                    EmployerId = "ABCDE",
                    Submitted = false
                },
                new Report()
                {
                    Id = Guid.NewGuid(),
                    ReportingPeriod = "1617",
                    EmployerId = "ABCDE",
                    Submitted = true
                },
                new Report()
                {
                    Id = Guid.NewGuid(),
                    ReportingPeriod = "1516",
                    EmployerId = "ABCDE",
                    Submitted = true
                },
                new Report()
                {
                    Id = Guid.NewGuid(),
                    ReportingPeriod = "1718",
                    EmployerId = "VWXYZ",
                    Submitted = false
                }
            }).AsEnumerable();
            _reportDtoList = (new List<ReportDto>()
            {
                new ReportDto()
                {
                    Id = Guid.NewGuid(),
                    ReportingPeriod = "1718",
                    EmployerId = "ABCDE",
                    Submitted = false
                },
                new ReportDto()
                {
                    Id = Guid.NewGuid(),
                    ReportingPeriod = "1617",
                    EmployerId = "ABCDE",
                    Submitted = true
                },
                new ReportDto()
                {
                    Id = Guid.NewGuid(),
                    ReportingPeriod = "1516",
                    EmployerId = "ABCDE",
                    Submitted = true
                },
                new ReportDto()
                {
                    Id = Guid.NewGuid(),
                    ReportingPeriod = "1718",
                    EmployerId = "VWXYZ",
                    Submitted = false
                }
            }).AsEnumerable();

        }

        [Test]
        public void When_The_Mapping_Is_Registered_Then_Is_Valid()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
            
        }

        [Test]
        public void When_I_Map_From_ReportDto_And_Json_is_Valid_Then_Return_Report()
        {
            //Mapper.AssertConfigurationIsValid();

            var reportDto = new ReportDto()
            {
               EmployerId = "ABCDE",
                ReportingData = "{\"OrganisationName\":\"Organisation 1\",\"Questions\":\"\",\"Submitted\":null}",
                ReportingPeriod = "1617",
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Submitted = true
            };

            var mappedReport = _mapper.Map<ReportDto, Report>(reportDto);

            Assert.AreEqual(mappedReport.OrganisationName, "Organisation 1");
         Assert.AreEqual(mappedReport.Submitted,reportDto.Submitted);
            Assert.AreEqual(mappedReport.EmployerId,reportDto.EmployerId);
            Assert.AreEqual(mappedReport.Id,reportDto.Id);
            Assert.AreEqual(mappedReport.ReportingPeriod,reportDto.ReportingPeriod);
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
                Submitted = true
            };

            var mappedReportDto = _mapper.Map<Report, ReportDto>(report);

            Assert.AreEqual(mappedReportDto.ReportingData, "{\"OrganisationName\":\"Organisation 1\",\"Questions\":null,\"Submitted\":null}");
            Assert.AreEqual(mappedReportDto.Submitted, report.Submitted);
            Assert.AreEqual(mappedReportDto.EmployerId, report.EmployerId);
            Assert.AreEqual(mappedReportDto.Id, report.Id);
            Assert.AreEqual(mappedReportDto.ReportingPeriod, report.ReportingPeriod);
        }

    }
}
