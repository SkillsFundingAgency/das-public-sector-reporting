﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests
{
    [TestFixture]
    public class Given_I_Request_The_Report_Create_Page : ReportControllerTestBase
    {
        [TestCase(true)]
        [TestCase(false)]
        public void And_The_Report_Creation_Is_Successful_Then_Redirect_To_IsLocalAuthority(bool isLocalAuthority)
        {
            // arrange
            var url = "report/IsLocalAuthority";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

            _mockReportService.Setup(s => s.CreateReport(It.IsAny<string>(), It.IsAny<UserModel>(), isLocalAuthority));

            // act
            var result = _controller.PostCreate();


            // assert
            _mockUrlHelper.VerifyAll();

            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(url, redirectResult.Url);
            Assert.AreEqual("IsLocalAuthority", actualContext.Action);
            Assert.AreEqual("Report", actualContext.Controller);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void And_The_Report_Creation_Fails_Then_Throw_Error(bool isLocalAuthority)
        {

            _mockReportService.Setup(s => s.CreateReport(It.IsAny<string>(), It.IsAny<UserModel>(), isLocalAuthority))
                .Throws(new Exception("Unable to create Report"));

            // act
            var result = _controller.PostCreate();

            // assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

    }
}
