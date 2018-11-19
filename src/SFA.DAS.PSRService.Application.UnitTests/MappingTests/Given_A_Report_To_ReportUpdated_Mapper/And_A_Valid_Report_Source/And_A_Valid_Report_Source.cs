using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.UnitTests.MappingTests.Given_A_Report_To_ReportUpdated_Mapper.And_A_Valid_Report_Source
{
    [ExcludeFromCodeCoverage]
    public abstract class And_A_Valid_Report_Source
    : Given_A_Report_To_ReportUpdated_Mapper
    {
        protected Report SourceReport;

        protected override void Given()
        {
            base.Given();
            
            SourceReport = ReportBuilder.BuildValidSubmittedReport();
            var reportingData = 
                JsonConvert
                    .DeserializeObject<ReportingData>(
                        new QuestionConfigWithValidAnswersProvider()
                            .GetQuestionConfig());


            SourceReport.Sections = reportingData.Questions;
            SourceReport.OrganisationName = reportingData.OrganisationName;
            SourceReport.ReportingPercentages = reportingData.ReportingPercentages;
        }
    }
}