using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification
{
    public class YourEmployeesVerifier
    {
        private readonly IEnumerable<JToken> yourEmployeesQuestions;

        public YourEmployeesVerifier(string reportData)
        {
            yourEmployeesQuestions = JObject.Parse(reportData)["Questions"]
                .SingleOrDefault(s => Extensions.Value<String>(s["Id"]) == "ReportNumbers")
                ["SubSections"].SingleOrDefault(s => s["Id"].Value<String>() == "YourEmployees")
                ["Questions"]
                .ToList();
        }

        public QuestionVerifier AtStartQuestion =>
            new QuestionVerifier(
                yourEmployeesQuestions
                    .Single(q => q["Id"].Value<String>() == "atStart"));

        public QuestionVerifier AtEndQuestion =>
            new QuestionVerifier(
                yourEmployeesQuestions
                    .Single(q => q["Id"].Value<String>() == "atEnd"));

        public QuestionVerifier NewThisPeriodQuestion =>
            new QuestionVerifier(
                yourEmployeesQuestions
                    .Single(q => q["Id"].Value<String>() == "newThisPeriod"));
    }
}