using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification
{
    [ExcludeFromCodeCoverage]
    public sealed class OutlineActionsVerifier
    : ReportFactorsVerifier
    {
        public OutlineActionsVerifier(string reportData) : base(reportData)
        {
        }

        protected override string SectionName => "OutlineActions";
        protected override string QuestionName => "OutlineActions";
    }
}