using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests
{
    [TestFixture]
    public class Given_I_Request_The_Report_Create_Page : ReportControllerTestBase
    {
        [Test]
        public void And_The_Report_Creation_Is_Successful_Then_Redirect_To_Edit()
        {
            // arrange
            var url = "report/Edit";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

            _mockReportService.Setup(s => s.CreateReport(It.IsAny<string>(), It.IsAny<UserModel>()));

            // act
            var result = _controller.PostCreate();


            // assert
            _mockUrlHelper.VerifyAll();

            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(url, redirectResult.Url);
            Assert.AreEqual("Edit", actualContext.Action);
            Assert.AreEqual("Report", actualContext.Controller);
        }

        [Test]
        public void And_The_Report_Creation_Fails_Then_Throw_Error()
        {
         
            _mockReportService.Setup(s => s.CreateReport(It.IsAny<string>(), It.IsAny<UserModel>()))
                .Throws(new Exception("Unable to create Report"));
        
            // act
            var result = _controller.PostCreate();
            
            // assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

    }
}
