using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification
{
    [ExcludeFromCodeCoverage]
    public class ReportVerifier
    {
        private readonly ReportDto _report;

        public ReportVerifier(ReportDto report)
        {
            _report = report;
        }
        public OrganisationNameVerifier OrganisationName => new OrganisationNameVerifier(_report.ReportingData);

        public YourEmployeesVerifier YourEmployees => new YourEmployeesVerifier(_report.ReportingData);

        public YourApprenticesVerifier YourApprentices => new YourApprenticesVerifier(_report.ReportingData);

        public FullTimeEquivalentsVerifier FullTimeEquivalents => new FullTimeEquivalentsVerifier(_report.ReportingData);

        public QuestionVerifier OutlineActions => new OutlineActionsVerifier(_report.ReportingData).SingleQuestionVerifier;
        public QuestionVerifier Challenges => new ChallengesVerifier(_report.ReportingData).SingleQuestionVerifier;
        public QuestionVerifier TargetPlans => new TargetPlansVerifier(_report.ReportingData).SingleQuestionVerifier;
        public QuestionVerifier AnythingElse => new AnythingElseVerifier(_report.ReportingData).SingleQuestionVerifier;

        internal static ReportVerifier VerifyReport(ReportDto report)
        {
            return new ReportVerifier(report);
        }
    }
}