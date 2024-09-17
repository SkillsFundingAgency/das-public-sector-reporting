using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests;

[TestFixture]
public class Given_I_Request_The_Report_IsLocalAuthority_Page : ReportControllerTestBase
{
    [TestCase(true)]
    [TestCase(false)]
    public void And_The_Report_IsLocalAuthority_Is_Successful_Then_Redirect_To_Edit(bool isLocalAuthority)
    {
        // arrange
        var url = "report/Edit";
        UrlActionContext actualContext = null;

        MockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

        MockReportService.Setup(s => s.CreateReport(It.IsAny<string>(), It.IsAny<UserModel>(), isLocalAuthority));

        MockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns((Report)null);

        // act
        var result = Controller.PostIsLocalAuthority(new IsLocalAuthorityViewModel { IsLocalAuthority = isLocalAuthority });

        // assert
        MockUrlHelper.VerifyAll();

        var redirectResult = result as RedirectResult;
        redirectResult.Should().NotBeNull();
        redirectResult.Url.Should().Be(url);
        actualContext.Action.Should().Be("Edit");
        actualContext.Controller.Should().BeNull("Report");
    }

    [TestCase(true)]
    [TestCase(false)]
    public void And_The_Report_Creation_Fails_Then_Throw_Error(bool isLocalAuthority)
    {
        MockReportService.Setup(s => s.CreateReport(It.IsAny<string>(), It.IsAny<UserModel>(), isLocalAuthority))
            .Throws(new Exception("Unable to create Report"));

        // act
        var result = Controller.PostIsLocalAuthority(new IsLocalAuthorityViewModel { IsLocalAuthority = isLocalAuthority });

        // assert
        result.Should().BeOfType<BadRequestResult>();
    }
}