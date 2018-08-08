using System;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification
{
    [ExcludeFromCodeCoverage]
    public class YourApprenticesVerifier : ReportNumbersVerifier
    {
        public YourApprenticesVerifier(string reportReportingData) 
            : base(reportReportingData)
        {
        }

        protected override string SectionName => "YourApprentices";
    }
}