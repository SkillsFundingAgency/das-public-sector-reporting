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
        public void When_I_Map_From_AuditRecordDto_And_Json_is_Valid_Then_Return_AuditRecord()
        {
            var updatedTime = new DateTime(2018, 06, 28, 11, 12, 13,
                DateTimeKind.Unspecified);

            var reportId = Guid.NewGuid();
            var organisationName = "ESFA";


            var updateUser = new User { Id = Guid.NewGuid(), Name = "Me" };
            //var updatedByUserId = Guid.NewGuid();
            //var updatedByUserName = "Me";

            var employmentStarts = "11.00";
            var totalHeadCount = "22.00";
            var newThisPeriod = "33.00";

            var sectionOneId = "101";
            var sectionOneTitle = "title 101";
            var sectionOneSummaryText = "summary text";
            //private string _validReportJson = "{\"OrganisationName\":\"THE INSTITUTION OF OCCUPATIONAL SAFETY AND HEALTH\",\"Questions\":[{\"SubSections\":[{\"SubSections\":null,\"Questions\":[{\"Id\":\"atStart\",\"Answer\":\"321\",\"Optional\":false,\"Type\":0},{\"Id\":\"atEnd\",\"Answer\":\"36\",\"Optional\":false,\"Type\":0},{\"Id\":\"newThisPeriod\",\"Answer\":\"32\",\"Optional\":false,\"Type\":0}],\"Id\":\"YourEmployees\",\"Title\":\"Your employees\",\"SummaryText\":\"Number of employees who work in England\"},{\"SubSections\":null,\"Questions\":[{\"Id\":\"atStart\",\"Answer\":\"142\",\"Optional\":false,\"Type\":0},{\"Id\":\"atEnd\",\"Answer\":\"152\",\"Optional\":false,\"Type\":0},{\"Id\":\"newThisPeriod\",\"Answer\":\"32\",\"Optional\":false,\"Type\":0}],\"Id\":\"YourApprentices\",\"Title\":\"Your apprentices\",\"SummaryText\":\"Number of apprentices who work in England\"},{\"SubSections\":null,\"Questions\":[{\"Id\":\"atStart\",\"Answer\":\"\",\"Optional\":true,\"Type\":0}],\"Id\":\"FullTimeEquivalent\",\"Title\":\"Full time equivalents\",\"SummaryText\":\"Number of full-time equivalents who work in England (optional)\"}],\"Questions\":null,\"Id\":\"ReportNumbers\",\"Title\":\"Report numbers in the following categories\",\"SummaryText\":null},{\"SubSections\":[{\"SubSections\":null,\"Questions\":[{\"Id\":\"OutlineActions\",\"Answer\":\"dsadsada\",\"Optional\":false,\"Type\":2}],\"Id\":\"OutlineActions\",\"Title\":\"Outline any actions you have taken to help you progress towards meeting the public sector target\",\"SummaryText\":\"Outline any actions you have taken to help you progress towards meeting the public sector target\"},{\"SubSections\":null,\"Questions\":[{\"Id\":\"Challenges\",\"Answer\":\"dsadsad\",\"Optional\":false,\"Type\":2}],\"Id\":\"Challenges\",\"Title\":\"Tell us about any challenges you have faced in your efforts to meet the target\",\"SummaryText\":\"Tell us about any challenges you have faced in your efforts to meet the target\"},{\"SubSections\":null,\"Questions\":[{\"Id\":\"TargetPlans\",\"Answer\":\"dsadsadas\",\"Optional\":false,\"Type\":2}],\"Id\":\"TargetPlans\",\"Title\":\"How are you planning to ensure you meet the target in future?\",\"SummaryText\":\"How are you planning to ensure you meet the target in future?\"},{\"SubSections\":null,\"Questions\":[{\"Id\":\"AnythingElse\",\"Answer\":\"\",\"Optional\":true,\"Type\":2}],\"Id\":\"AnythingElse\",\"Title\":\"Do you have anything else you want to tell us? (optional)\",\"SummaryText\":\"Do you have anything else you want to tell us? (optional)\"}],\"Questions\":null,\"Id\":\"Factors\",\"Title\":\"Factors that impacted your ability to meet the target\",\"SummaryText\":null}],\"Submitted\":null,\"ReportingPercentages\":{\"EmploymentStarts\":100.0,\"TotalHeadCount\":422.22222222222222222222222222,\"NewThisPeriod\":9.968847352024922118380062310}}";
            //private string _invalidReportJson = "{\"OrganisationName\":\"THE INSTITUTION OF OCCUPATIONAL SAFETY AND HEALTH\",\"Questions\":[{\"SubSections\":[{\"SubSections\":null,\"Questions\":[{\"Id\":\"atStart\",\"Answer\":\"321\",\"Optional\":false,\"Type\":0},{\"Id\":\"atEnd\",\"Answer\":\"36\",\"Optional\":false,\"Type\":0},{\"Id\":\"newThisPeriod\",\"Answer\":\"32\",\"Optional\":false,\"Type\":0}],\"Id\":\"YourEmployees\",\"Title\":\"Your employees\",\"SummaryText\":\"Number of employees who work in England\"},{\"SubSections\":null,\"Questions\":[{\"Id\":\"atStart\",\"Answer\":\"142\",\"Optional\":false,\"Type\":0},{\"Id\":\"atEnd\",\"Answer\":\"\",\"Optional\":false,\"Type\":0},{\"Id\":\"newThisPeriod\",\"Answer\":\"\",\"Optional\":false,\"Type\":0}],\"Id\":\"YourApprentices\",\"Title\":\"Your apprentices\",\"SummaryText\":\"Number of apprentices who work in England\"},{\"SubSections\":null,\"Questions\":[{\"Id\":\"atStart\",\"Answer\":\"\",\"Optional\":true,\"Type\":0}],\"Id\":\"FullTimeEquivalent\",\"Title\":\"Full time equivalents\",\"SummaryText\":\"Number of full-time equivalents who work in England (optional)\"}],\"Questions\":null,\"Id\":\"ReportNumbers\",\"Title\":\"Report numbers in the following categories\",\"SummaryText\":null},{\"SubSections\":[{\"SubSections\":null,\"Questions\":[{\"Id\":\"OutlineActions\",\"Answer\":\"dsadsada\",\"Optional\":false,\"Type\":2}],\"Id\":\"OutlineActions\",\"Title\":\"Outline any actions you have taken to help you progress towards meeting the public sector target\",\"SummaryText\":\"Outline any actions you have taken to help you progress towards meeting the public sector target\"},{\"SubSections\":null,\"Questions\":[{\"Id\":\"Challenges\",\"Answer\":\"dsadsad\",\"Optional\":false,\"Type\":2}],\"Id\":\"Challenges\",\"Title\":\"Tell us about any challenges you have faced in your efforts to meet the target\",\"SummaryText\":\"Tell us about any challenges you have faced in your efforts to meet the target\"},{\"SubSections\":null,\"Questions\":[{\"Id\":\"TargetPlans\",\"Answer\":\"dsadsadas\",\"Optional\":false,\"Type\":2}],\"Id\":\"TargetPlans\",\"Title\":\"How are you planning to ensure you meet the target in future?\",\"SummaryText\":\"How are you planning to ensure you meet the target in future?\"},{\"SubSections\":null,\"Questions\":[{\"Id\":\"AnythingElse\",\"Answer\":\"\",\"Optional\":true,\"Type\":2}],\"Id\":\"AnythingElse\",\"Title\":\"Do you have anything else you want to tell us? (optional)\",\"SummaryText\":\"Do you have anything else you want to tell us? (optional)\"}],\"Questions\":null,\"Id\":\"Factors\",\"Title\":\"Factors that impacted your ability to meet the target\",\"SummaryText\":null}],\"Submitted\":null,\"ReportingPercentages\":{\"EmploymentStarts\":100.0,\"TotalHeadCount\":422.22222222222222222222222222,\"NewThisPeriod\":9.968847352024922118380062310}}";

            var questionOneId = "atStart";
            var questionOneAnswer = "1";
            var questionOneOptional = false;
            var questionOneType = QuestionType.Number;

            var dto = new AuditRecordDto
            {
                Id = 1,
                ReportId = reportId,
                ReportingData = $"{{ " +
                    $"\"OrganisationName\": \"{organisationName}\", " +
                    $"\"ReportingPercentages\": {{ " +
                    $"    \"EmploymentStarts\": \"{employmentStarts}\", TotalHeadCount: \"{ totalHeadCount}\", NewThisPeriod: \"{newThisPeriod}\"" +
                    $"  }}," +
                    $"\"Questions\": [ " +
                    $"    {{ " +
                    $"        \"SubSections\": [ " +
                    $"            {{ " +
                    $"            \"SubSections\": null," +
                    $"                 \"Questions\": [ " +
                    $"                         {{ " +
                    $"                            \"Id\": \"{questionOneId}\", " +
                    $"                            \"Answer\": \"{questionOneAnswer}\", " +
                    $"                            \"Optional\": {questionOneOptional.ToString().ToLower()}, " +
                    $"                            \"Type\": {(int)questionOneType} " +
                    $"                        }} " +
                    $"                      ]," +
                    $"            }} " +
                    $"        ]," +
                    $"        Id: \"{sectionOneId}\", Title: \"{sectionOneTitle}\", SummaryText: \"{sectionOneSummaryText}\" " +
                    $"    }}" +
                    $"  ]," +
                    $"}}",
                UpdatedUtc = updatedTime,
                UpdatedBy = $"{{ Id: \"{updateUser.Id}\", Name: \"{updateUser.Name}\" }}"
            };

            //Console.WriteLine(dto.ReportingData);

            var auditRecord = (AuditRecord)_mapper.Map(dto, typeof(AuditRecordDto), typeof(AuditRecord));

            Assert.AreEqual(updateUser.Id, auditRecord.UpdatedBy.Id);
            Assert.AreEqual(updateUser.Name, auditRecord.UpdatedBy.Name);
            Assert.AreEqual(organisationName, auditRecord.OrganisationName);

            Assert.AreEqual(employmentStarts, auditRecord.ReportingPercentages.EmploymentStarts);
            Assert.AreEqual(totalHeadCount, auditRecord.ReportingPercentages.TotalHeadCount);
            Assert.AreEqual(newThisPeriod, auditRecord.ReportingPercentages.NewThisPeriod);

            Assert.AreEqual(sectionOneId, auditRecord.Sections.First().Id);
            Assert.AreEqual(sectionOneTitle, auditRecord.Sections.First().Title);
            Assert.AreEqual(sectionOneSummaryText, auditRecord.Sections.First().SummaryText);

            Assert.AreEqual(1, auditRecord.Sections.First().SubSections.Count());
            Assert.AreEqual(1, auditRecord.Sections.First().SubSections.First().Questions.Count());

            var questionOne = auditRecord.Sections.First().SubSections.First().Questions.First();

            Assert.AreEqual(questionOneId, questionOne.Id);
            Assert.AreEqual(questionOneAnswer, questionOne.Answer);
            Assert.AreEqual(questionOneOptional, questionOne.Optional);
            Assert.AreEqual(questionOneType, questionOne.Type);

            Assert.AreEqual(updatedTime.Ticks, auditRecord.UpdatedUtc.Ticks);

            var calculatedLocalTime = updatedTime.AddHours(1);
            Assert.AreEqual(calculatedLocalTime.Ticks, auditRecord.UpdatedLocal.Ticks);

            //TODO: Test local time in seperate test method
            //TODO: Fix the bugs below

            Assert.AreEqual(DateTimeKind.Local, auditRecord.UpdatedLocal.Kind);
            Assert.AreEqual(DateTimeKind.Utc, auditRecord.UpdatedUtc.Kind);
        }
    }
}