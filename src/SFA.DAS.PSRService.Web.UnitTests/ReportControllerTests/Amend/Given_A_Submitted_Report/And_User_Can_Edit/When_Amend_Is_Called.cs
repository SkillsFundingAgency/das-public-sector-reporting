using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.PSRService.Application.ReportHandlers;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Amend.Given_A_Submitted_Report.And_User_Can_Edit;

[ExcludeFromCodeCoverage]
[TestFixture]
public class WhenAmendIsCalled : And_User_Can_Edit
{
    private IActionResult _result;
    private const string ExpectedUrl = "Report/Edit";

    protected override void Given()
    {
        base.Given();

        MockUrlHelper
            .Setup(m => m.Action(It.Is<UrlActionContext>(
                ctx =>
                    ctx.Action.Equals("Edit", StringComparison.OrdinalIgnoreCase)
                    && ctx.Controller.Equals("Report", StringComparison.OrdinalIgnoreCase))))
            .Returns(ExpectedUrl);
    }

    protected override async Task When()
    {
        await base.When();

        _result = await Controller.Amend();
    }

    [Test]
    public void Then_UnSubmit_Command_Is_Called()
    {
        MockMediatr.Verify(m => m.Send(It.IsAny<UnSubmitReportRequest>(), It.IsAny<CancellationToken>()));
    }

    [Test]
    public void Then_I_Am_Redirected_To_EditPage()
    {
        _result
            .Should()
            .BeAssignableTo<RedirectResult>();

        ((RedirectResult)_result)
            .Url
            .Should()
            .BeEquivalentTo(ExpectedUrl);
    }
}