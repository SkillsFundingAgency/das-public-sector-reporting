using System;
using System.IO;
using System.Reflection;
using SFA.DAS.PSRService.Application.Interfaces;

namespace SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests
{
    public sealed class TestQuestionConfigProvider
    :QuestionConfigProvider
    {
        private Lazy<string> newlyCreatedReportQuestionConfig = new Lazy<string>(() => readConfigFromResource(@"SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests.new_report_question_config.json"));
        private Lazy<string> questionConfigWithValidAnswers = new Lazy<string>(() => readConfigFromResource(@"SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests.report_question_config_with_valid_answers.json"));

        public string GetNewlyCreatedReportQuestionConfig() => newlyCreatedReportQuestionConfig.Value;
        public string GetReportQuestionConfigWithValidAnswers() => questionConfigWithValidAnswers.Value;

        private static string readConfigFromResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            string readData;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
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
