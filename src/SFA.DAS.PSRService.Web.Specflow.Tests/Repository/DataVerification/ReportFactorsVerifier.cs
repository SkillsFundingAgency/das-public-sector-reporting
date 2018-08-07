using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification
{
    public abstract class ReportFactorsVerifier
    {
        private IEnumerable<JToken> reportNumbersQuestions;

        public ReportFactorsVerifier(string reportData)
        {
            reportNumbersQuestions = JObject.Parse(reportData)["Questions"]
                .SingleOrDefault(s => Extensions.Value<String>(s["Id"]) == "Factors")
                ["SubSections"].SingleOrDefault(s => s["Id"].Value<String>() == SectionName)
                ["Questions"]
                .ToList();
        }

        protected abstract string SectionName { get; }
        protected abstract string QuestionName { get; }

        public QuestionVerifier SingleQuestionVerifier =>
            new QuestionVerifier(
                reportNumbersQuestions
                    .Single(q => q["Id"].Value<String>() == QuestionName));
    }
}