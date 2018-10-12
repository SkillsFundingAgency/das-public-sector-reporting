using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification
{
    [ExcludeFromCodeCoverage]
    public sealed class TargetPlansVerifier
        : ReportFactorsVerifier
    {
        public TargetPlansVerifier(string reportData) : base(reportData)
        {
        }

        protected override string SectionName => "TargetPlans";
        protected override string QuestionName => "TargetPlans";
    }
}