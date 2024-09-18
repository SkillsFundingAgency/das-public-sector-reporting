using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests;

[TestFixture]
public class Given_I_Request_The_Report_IsLocalAuthority_Page : ReportControllerTestBase
{
    [TestCase(true)]
    [TestCase(false)]
    public async Task And_The_Report_IsLocalAuthority_Is_Successful_Then_Redirect_To_Edit(bool isLocalAuthority)
    {
        // arrange
        MockReportService.Setup(s => s.CreateReport(It.IsAny<string>(), It.IsAny<UserModel>(), isLocalAuthority));
        MockReportService.Setup(s => s.CanBeEdited(It.IsAny<Report>())).Returns(true);
        MockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new Report { IsLocalAuthority = isLocalAuthority });

        // act
        var result = await Controller.PostIsLocalAuthority(new IsLocalAuthorityViewModel { IsLocalAuthority = isLocalAuthority });

        // assert
        MockUrlHelper.VerifyAll();

        var redirectResult = result as RedirectToActionResult;
        redirectResult.Should().NotBeNull();
        redirectResult.ActionName.Should().Be("Edit");
        redirectResult.ControllerName.Should().Be("Report");
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task And_The_Report_Creation_Fails_Then_Throw_Error(bool isLocalAuthority)
    {
        MockReportService.Setup(s => s.CreateReport(It.IsAny<string>(), It.IsAny<UserModel>(), isLocalAuthority))
            .Throws(new Exception("Unable to create Report"));

        // act
        var result = await Controller.PostIsLocalAuthority(new IsLocalAuthorityViewModel { IsLocalAuthority = isLocalAuthority });

        // assert
        result.Should().BeOfType<BadRequestResult>();
    }
}