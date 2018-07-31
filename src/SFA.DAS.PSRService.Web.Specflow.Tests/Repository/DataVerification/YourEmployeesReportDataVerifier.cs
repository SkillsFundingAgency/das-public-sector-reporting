using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification
{
    public class YourEmployeesReportDataVerifier
    {
        private IEnumerable<JToken> yourEmployeesQuestions;

        public YourEmployeesReportDataVerifier(string reportData)
        {
            yourEmployeesQuestions = JObject.Parse(reportData)["Questions"]
                .SingleOrDefault(s => Extensions.Value<String>(s["Id"]) == "ReportNumbers")
                ["SubSections"].SingleOrDefault(s => s["Id"].Value<String>() == "YourEmployees")
                ["Questions"]
                .ToList();
        }

        public QuestionVerifier AtStart =>
            new QuestionVerifier(
                yourEmployeesQuestions
                    .Single(q => q["Id"].Value<String>() == "atStart"));

        public QuestionVerifier AtEnd =>
            new QuestionVerifier(
                yourEmployeesQuestions
                    .Single(q => q["Id"].Value<String>() == "atEnd"));

        public QuestionVerifier NewThisPeriod =>
            new QuestionVerifier(
                yourEmployeesQuestions
                    .Single(q => q["Id"].Value<String>() == "newThisPeriod"));
    }
}