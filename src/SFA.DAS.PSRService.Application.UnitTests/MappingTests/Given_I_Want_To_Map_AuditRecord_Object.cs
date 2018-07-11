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
    public class Given_I_Want_To_Map_AuditRecord_Object
    {
        private IMapper _mapper;

        private DateTime _updatedTimeInSummerWithKindUnspecified = new DateTime(2018, 06, 28, 11, 12, 13,
                 DateTimeKind.Unspecified);
        private DateTime _updatedTimeInWinterWithKindUnspecified = new DateTime(2018, 02, 01, 15, 50, 15,
                 DateTimeKind.Unspecified);

        private Guid _reportId = Guid.NewGuid();
        private string _organisationName = "ESFA";

        private User _updateUser = new User { Id = Guid.NewGuid(), Name = "Me" };

        private string _employmentStarts = "11.00";
        private string _totalHeadCount = "22.00";
        private string _newThisPeriod = "33.00";

        private string _sectionOneId = "101";
        private string _sectionOneTitle = "title 101";
        private string _sectionOneSummaryText = "summary text";

        private string _questionOneId = "atStart";
        private string _questionOneAnswer = "1";
        private bool _questionOneOptional = false;
        private QuestionType _questionOneType = QuestionType.Number;

        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AuditRecordMappingProfile>());
            _mapper = config.CreateMapper();
        }

        [Test]
        public void When_The_Mapping_Is_Registered_Then_Is_Valid()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Test]
        public void When_I_Map_From_AuditRecordDto_And_Json_Is_Valid_Then_Return_AuditRecord()
        {
            var dto = GetValidAuditRecordDto();

            var auditRecord = (AuditRecord)_mapper.Map(dto, typeof(AuditRecordDto), typeof(AuditRecord));

            Assert.AreEqual(_updateUser.Id, auditRecord.UpdatedBy.Id);
            Assert.AreEqual(_updateUser.Name, auditRecord.UpdatedBy.Name);
            Assert.AreEqual(_organisationName, auditRecord.OrganisationName);

            Assert.AreEqual(_employmentStarts, auditRecord.ReportingPercentages.EmploymentStarts);
            Assert.AreEqual(_totalHeadCount, auditRecord.ReportingPercentages.TotalHeadCount);
            Assert.AreEqual(_newThisPeriod, auditRecord.ReportingPercentages.NewThisPeriod);

            Assert.AreEqual(_sectionOneId, auditRecord.Sections.First().Id);
            Assert.AreEqual(_sectionOneTitle, auditRecord.Sections.First().Title);
            Assert.AreEqual(_sectionOneSummaryText, auditRecord.Sections.First().SummaryText);

            Assert.AreEqual(1, auditRecord.Sections.First().SubSections.Count());
            Assert.AreEqual(1, auditRecord.Sections.First().SubSections.First().Questions.Count());

            var questionOne = auditRecord.Sections.First().SubSections.First().Questions.First();

            Assert.AreEqual(_questionOneId, questionOne.Id);
            Assert.AreEqual(_questionOneAnswer, questionOne.Answer);
            Assert.AreEqual(_questionOneOptional, questionOne.Optional);
            Assert.AreEqual(_questionOneType, questionOne.Type);

            Assert.AreEqual(_updatedTimeInSummerWithKindUnspecified.Ticks, auditRecord.UpdatedUtc.Ticks);
        }
        
        [Test]
        public void When_I_Map_From_AuditRecordDto_And_UpdatedDate_Is_Not_Daylight_Saving_Then_AuditRecord_Local_Time_Is_Same_As_Original()
        {
            var dto = GetValidAuditRecordDto();
            dto.UpdatedUtc = _updatedTimeInWinterWithKindUnspecified;

            var auditRecord = (AuditRecord)_mapper.Map(dto, typeof(AuditRecordDto), typeof(AuditRecord));

            //Assert.AreEqual(_updatedTimeInSummerWithKindUnspecified.Ticks, auditRecord.UpdatedUtc.Ticks);
            Assert.AreEqual(_updatedTimeInWinterWithKindUnspecified.Ticks, auditRecord.UpdatedUtc.Ticks);

            //TODO: Test with an input date that is NOT summertime
            var calculatedLocalTime = _updatedTimeInWinterWithKindUnspecified;
            Assert.AreEqual(calculatedLocalTime.Ticks, auditRecord.UpdatedLocal.Ticks);
        }
        
        [Test]
        public void When_I_Map_From_AuditRecordDto_Then_AuditRecord_UpdatedUtc_Time_Is_Of_Kind_Utc()
        {
            var dto = GetValidAuditRecordDto();

            var auditRecord = (AuditRecord)_mapper.Map(dto, typeof(AuditRecordDto), typeof(AuditRecord));

            Assert.AreEqual(DateTimeKind.Utc, auditRecord.UpdatedUtc.Kind);
        }

        [Test]
        public void When_I_Map_From_AuditRecordDto_Then_AuditRecord_UpdatedLocal_Time_Is_Of_Kind_Local()
        {
            var dto = GetValidAuditRecordDto();

            var auditRecord = (AuditRecord)_mapper.Map(dto, typeof(AuditRecordDto), typeof(AuditRecord));
                       
            Assert.AreEqual(DateTimeKind.Local, auditRecord.UpdatedLocal.Kind);
        }

        private AuditRecordDto GetValidAuditRecordDto()
        {
            var dto = new AuditRecordDto
            {
                Id = 1,
                ReportId = _reportId,
                ReportingData = $"{{ " +
                    $"\"OrganisationName\": \"{_organisationName}\", " +
                    $"\"ReportingPercentages\": {{ " +
                    $"    \"EmploymentStarts\": \"{_employmentStarts}\", TotalHeadCount: \"{ _totalHeadCount}\", NewThisPeriod: \"{_newThisPeriod}\"" +
                    $"  }}," +
                    $"\"Questions\": [ " +
                    $"    {{ " +
                    $"        \"SubSections\": [ " +
                    $"            {{ " +
                    $"            \"SubSections\": null," +
                    $"                 \"Questions\": [ " +
                    $"                         {{ " +
                    $"                            \"Id\": \"{_questionOneId}\", " +
                    $"                            \"Answer\": \"{_questionOneAnswer}\", " +
                    $"                            \"Optional\": {_questionOneOptional.ToString().ToLower()}, " +
                    $"                            \"Type\": {(int)_questionOneType} " +
                    $"                        }} " +
                    $"                      ]," +
                    $"            }} " +
                    $"        ]," +
                    $"        Id: \"{_sectionOneId}\", Title: \"{_sectionOneTitle}\", SummaryText: \"{_sectionOneSummaryText}\" " +
                    $"    }}" +
                    $"  ]," +
                    $"}}",
                UpdatedUtc = _updatedTimeInSummerWithKindUnspecified,
                UpdatedBy = $"{{ Id: \"{_updateUser.Id}\", Name: \"{_updateUser.Name}\" }}"
            };
            return dto;
        }
    }
}
