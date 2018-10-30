using System;
using System.IO;
using System.Reflection;
using SFA.DAS.PSRService.Application.Interfaces;

namespace SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests.Given_A_CreateReportHandler
{
    public sealed class TestQuestionConfigProvider
    :QuestionConfigProvider
    {
        private Lazy<string> questionConfig = new Lazy<string>(readConfigFromResource);

        public string GetQuestionConfig() => questionConfig.Value;

        private static string readConfigFromResource()
        {
            var assembly = Assembly.GetExecutingAssembly();

            string readData;

            using (var stream = assembly.GetManifestResourceStream(@"SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests.Given_A_CreateReportHandler.new_report_question_config.json"))
            {
                using (var reader = new StreamReader(stream))
                {
                    readData = reader.ReadToEnd();
                }
            }

            return readData;
        }
    }
}