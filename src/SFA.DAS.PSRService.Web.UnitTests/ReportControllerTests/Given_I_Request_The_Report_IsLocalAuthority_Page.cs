using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
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

        _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

        _mockReportService.Setup(s => s.CreateReport(It.IsAny<string>(), It.IsAny<UserModel>(), isLocalAuthority));

        _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns((Report)null);

        // act
        var result = _controller.PostIsLocalAuthority(new IsLocalAuthorityViewModel() { IsLocalAuthority = isLocalAuthority });

        // assert
        _mockUrlHelper.VerifyAll();

        var redirectResult = result as RedirectResult;
        Assert.IsNotNull(redirectResult);
        Assert.AreEqual(url, redirectResult.Url);
        Assert.AreEqual("Edit", actualContext.Action);
        Assert.AreEqual("Report", actualContext.Controller);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void And_The_Report_Creation_Fails_Then_Throw_Error(bool isLocalAuthority)
    {
        _mockReportService.Setup(s => s.CreateReport(It.IsAny<string>(), It.IsAny<UserModel>(), isLocalAuthority))
            .Throws(new Exception("Unable to create Report"));

        // act
        var result = _controller.PostIsLocalAuthority(new IsLocalAuthorityViewModel() { IsLocalAuthority = isLocalAuthority });

        // assert
        Assert.IsInstanceOf<BadRequestResult>(result);
    }
}