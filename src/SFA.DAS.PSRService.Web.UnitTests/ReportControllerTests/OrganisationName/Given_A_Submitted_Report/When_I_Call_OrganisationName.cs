using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.OrganisationName.Given_A_Submitted_Report;

[ExcludeFromCodeCoverage]
[TestFixture]
public sealed class WhenICallOrganisationName : Given_Report_Cannot_Be_Edited
{
    private IActionResult _result;

    protected override void When()
    {
        base.When();

        _result = Sut.OrganisationName(string.Empty);
    }

    [Test]
    public void Then_I_Am_Redirected_To_HomePage()
    {
        _result
            .Should()
            .BeAssignableTo<RedirectResult>();

        ((RedirectResult) _result)
            .Url
            .Should()
            .BeEquivalentTo(ExpectedUrl);
    }
}