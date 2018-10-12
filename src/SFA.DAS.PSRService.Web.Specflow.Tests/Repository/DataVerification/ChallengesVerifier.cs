using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification
{
    [ExcludeFromCodeCoverage]
    public sealed class ChallengesVerifier
        : ReportFactorsVerifier
    {
        public ChallengesVerifier(string reportData) : base(reportData)
        {
        }

        protected override string SectionName => "Challenges";
        protected override string QuestionName => "Challenges";
    }
}