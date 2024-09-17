using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Summary.Given_ReportService_Throws_An_Exception;

[ExcludeFromCodeCoverage]
public class Given_ReportService_Throws_Exception : GivenAReportController
{
    protected override void Given()
    {
        const string url = "Home/Index";

        MockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => _ = c).Verifiable("Url.Action was never called");

        MockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception("get report Error"));
    }
}

[ExcludeFromCodeCoverage]
[TestFixture]
public class When_Summary_Is_Called : Given_ReportService_Throws_Exception
{
    private IActionResult _result;

    protected override void When()
    {
        const string hashedAccountId = "ABC123";
        _result = Controller.Summary(hashedAccountId, "ReportError");
    }

    [Test]
    public void Then_Result_Is_BadRequestResult()
    {
        _result.Should().BeOfType<BadRequestResult>();
    }
}