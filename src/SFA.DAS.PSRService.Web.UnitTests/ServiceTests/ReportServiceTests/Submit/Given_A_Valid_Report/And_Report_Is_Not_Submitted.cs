using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.ReportServiceTests.Submit.Given_A_Valid_Report;

[ExcludeFromCodeCoverage]
public abstract class And_Report_Is_Not_Submitted : Given_A_Valid_Report
{
    protected And_Report_Is_Not_Submitted()
    {
        ValidNotSubmittedReport.Submitted = false;
    }
}