using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests;

[TestFixture]
public class Given_I_Request_The_Report_Create_Page : ReportControllerTestBase
{
    [TestCase(true)]
    [TestCase(false)]
    public void And_The_Report_Creation_Is_Successful_Then_Redirect_To_IsLocalAuthority(bool isLocalAuthority)
    {
        MockReportService.Setup(s => s.CreateReport(It.IsAny<string>(), It.IsAny<UserModel>(), isLocalAuthority));

        var result = (RedirectToActionResult)Controller.PostCreate();

        MockUrlHelper.VerifyAll();

        result.Should().NotBeNull();
        result.ActionName.Should().Be("IsLocalAuthority");
        result.ControllerName.Should().Be("Report");
    }
}