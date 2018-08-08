using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification
{
    [ExcludeFromCodeCoverage]
    public abstract class ReportFactorsVerifier
    {
        private readonly IEnumerable<JToken> _reportFactorsQuestions;

        protected ReportFactorsVerifier(string reportData)
        {
            _reportFactorsQuestions = JObject.Parse(reportData)["Questions"]
                .SingleOrDefault(s => Extensions.Value<String>(s["Id"]) == "Factors")
                ["SubSections"].SingleOrDefault(s => s["Id"].Value<String>() == SectionName)
                ["Questions"]
                .ToList();
        }

        protected abstract string SectionName { get; }
        protected abstract string QuestionName { get; }

        public QuestionVerifier SingleQuestionVerifier =>
            new QuestionVerifier(
                _reportFactorsQuestions
                    .Single(q => q["Id"].Value<String>() == QuestionName));
    }
}