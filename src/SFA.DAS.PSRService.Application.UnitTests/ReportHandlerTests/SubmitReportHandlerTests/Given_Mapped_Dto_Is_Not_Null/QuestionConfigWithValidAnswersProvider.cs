﻿using System;
using System.IO;
using System.Reflection;
using SFA.DAS.PSRService.Application.Interfaces;

namespace SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests.SubmitReportHandlerTests.Given_Mapped_Dto_Is_Not_Null
{
    public sealed class QuestionConfigWithValidAnswersProvider
        :QuestionConfigProvider
    {
        private Lazy<string> questionConfig = new Lazy<string>(readConfigFromResource);

        public string GetQuestionConfig() => questionConfig.Value;

        private static string readConfigFromResource()
        {
            var assembly = Assembly.GetExecutingAssembly();

            string readData;

            using (var stream = assembly.GetManifestResourceStream(@"SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests.SubmitReportHandlerTests.Given_Mapped_Dto_Is_Not_Null.question_config_with_valid_answers.json"))
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