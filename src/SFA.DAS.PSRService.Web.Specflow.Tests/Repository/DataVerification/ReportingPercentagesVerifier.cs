using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification
{
    [ExcludeFromCodeCoverage]
    public class ReportingPercentagesVerifier
    {
        private readonly JToken _reportingPercentages;

        public ReportingPercentagesVerifier(string reportData)
        {
            _reportingPercentages = JObject.Parse(reportData)["ReportingPercentages"];
        }

        public PropertyVerifier EmploymentStarts => new PropertyVerifier(_reportingPercentages["EmploymentStarts"]);
        public PropertyVerifier TotalHeadCount => new PropertyVerifier(_reportingPercentages["TotalHeadCount"]);
        public PropertyVerifier NewThisPeriod => new PropertyVerifier(_reportingPercentages["NewThisPeriod"]);
    }
}