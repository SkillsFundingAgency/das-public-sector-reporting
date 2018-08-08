using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification
{
    [ExcludeFromCodeCoverage]
    public abstract class ReportNumbersVerifier
    {
        private readonly IEnumerable<JToken> _numbersQuestions;

        protected ReportNumbersVerifier(string reportData)
        {
            _numbersQuestions = JObject.Parse(reportData)["Questions"]
                .SingleOrDefault(s => Extensions.Value<String>(s["Id"]) == "ReportNumbers")
                ["SubSections"].SingleOrDefault(s => s["Id"].Value<String>() == SectionName)
                ["Questions"]
                .ToList();
        }

        protected abstract string SectionName { get; }

        public QuestionVerifier AtStartQuestion =>
            new QuestionVerifier(
                _numbersQuestions
                    .Single(q => q["Id"].Value<String>() == "atStart"));

        public QuestionVerifier AtEndQuestion =>
            new QuestionVerifier(
                _numbersQuestions
                    .Single(q => q["Id"].Value<String>() == "atEnd"));

        public QuestionVerifier NewThisPeriodQuestion =>
            new QuestionVerifier(
                _numbersQuestions
                    .Single(q => q["Id"].Value<String>() == "newThisPeriod"));
    }
}