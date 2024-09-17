using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Summary.Given_Report_Does_Not_Exist;

[ExcludeFromCodeCoverage]
public class Given_Report_Does_Not_Exist : GivenAReportController
{
    protected override void Given()
    {
        MockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((Report)null);
    }
}

[ExcludeFromCodeCoverage]
[TestFixture]
public class When_Summary_Is_Called : Given_Report_Does_Not_Exist
{
    private IActionResult _result;

    protected override async Task  When()
    {
        const string hashedAccountId = "ABC123";
        _result = await Controller.Summary(hashedAccountId, "NoReport");
    }
}