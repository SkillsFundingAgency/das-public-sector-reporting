using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.OrganisationName.Given_A_Submitted_Report;

[ExcludeFromCodeCoverage]
[TestFixture]
public sealed class WhenICallOrganisationName : Given_Report_Cannot_Be_Edited
{
    private IActionResult _result;

    protected override async Task  When()
    {
        await base.When();

        _result = await Sut.OrganisationName(string.Empty);
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