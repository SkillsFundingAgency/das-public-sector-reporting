using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Summary.Given_Report_Does_Not_Exist;

[ExcludeFromCodeCoverage]
public class Given_Report_Does_Not_Exist : GivenAReportController
{
    protected override void Given()
    {
        MockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns((Report)null);
    }
}

[ExcludeFromCodeCoverage]
[TestFixture]
public class When_Summary_Is_Called : Given_Report_Does_Not_Exist
{
    private IActionResult _result;

    protected override void When()
    {
        const string hashedAccountId = "ABC123";
        _result = Controller.Summary(hashedAccountId, "NoReport");
    }
}