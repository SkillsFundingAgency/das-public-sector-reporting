using System;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification
{
    public class YourApprenticesVerifier : ReportNumbersVerifier
    {
        public YourApprenticesVerifier(string reportReportingData) 
            : base(reportReportingData)
        {
        }

        protected override string SectionName => "YourApprentices";
    }
}