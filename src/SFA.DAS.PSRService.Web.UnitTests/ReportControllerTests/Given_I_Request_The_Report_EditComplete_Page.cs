using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests;

[TestFixture]
public class Given_I_Request_The_Report_EditComplete_Page : ReportControllerTestBase
{
    [Test]
    public void Then_Show_EditComplete_View()
    {
        var result = Controller.EditComplete();

        result.Should().BeOfType<ViewResult>();
        ((ViewResult)result).ViewName.Should().Be("EditComplete");
    }
}