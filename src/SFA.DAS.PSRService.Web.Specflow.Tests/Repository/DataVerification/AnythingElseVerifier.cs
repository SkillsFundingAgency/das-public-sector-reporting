using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification
{
    [ExcludeFromCodeCoverage]
    public sealed class AnythingElseVerifier
        : ReportFactorsVerifier
    {
        public AnythingElseVerifier(string reportData) : base(reportData)
        {
        }

        protected override string SectionName => "AnythingElse";
        protected override string QuestionName => "AnythingElse";
    }
}