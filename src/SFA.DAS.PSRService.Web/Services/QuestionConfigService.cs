using System;
using System.IO;
using Microsoft.Extensions.FileProviders;
using SFA.DAS.PSRService.Application.Interfaces;

namespace SFA.DAS.PSRService.Web.Services
{
    public class QuestionConfigService
    :QuestionConfigProvider
    {
        private Lazy<string> _questionConfig;
        private IFileProvider _fileProvider;

        public QuestionConfigService(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));

            _questionConfig = new Lazy<string>(readConfigFromFile);
        }

        public string GetQuestionConfig()
        {
            return _questionConfig.Value;
        }

        private string readConfigFromFile()
        {
            var questionsConfig = _fileProvider.GetFileInfo("/QuestionConfig.json");

            using (var jsonContents = questionsConfig.CreateReadStream())
            using (StreamReader sr = new StreamReader(jsonContents))
            {
                return sr.ReadToEnd();
            }
        }
    }
}