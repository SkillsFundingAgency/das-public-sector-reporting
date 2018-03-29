using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests
{
    [TestFixture]
    public class Given_I_Request_The_Report_Submit_Page :ReportControllerTestBase
    {
       
        // Period is null tests
        //report doesnt exist
            
        [Test]
        public void The_Report_Is_Valid_To_Submit_Then_Submit()
        {
           
            _mockReportService.Setup(s => s.SubmitReport(It.IsAny<string>(),It.IsAny<string>(), It.IsAny<Submitted>())).Returns(SubmittedStatus.Submitted);
            _mockReportService.Setup(s => s.GetPeriod(It.IsAny<string>())).Returns(new CurrentPeriod());
            // act
            var result = _controller.Submit("1718");

            // assert
            Assert.AreEqual(typeof(ViewResult), result.GetType());
            var editViewResult = result as ViewResult;
            Assert.IsNotNull(editViewResult);
            Assert.AreEqual("Submitted", editViewResult.ViewName, "View name does not match, should be: List");


        }

        [Test]
        public void The_Report_Is_Not_Valid_To_Submit_Then_Redirect_Home()
        {
            
            // arrange
            var url = "home/Index";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url)
                .Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

           
            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(new Report());
            _mockReportService.Setup(s => s.IsSubmitValid(It.IsAny<Report>())).Returns(false);
            _mockReportService.Setup(s => s.SubmitReport(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Submitted>())).Returns(SubmittedStatus.Invalid);
            // act
            var result = _controller.Submit("1617");

            // assert
            _mockUrlHelper.VerifyAll();

            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(url, redirectResult.Url);
            Assert.AreEqual("Index", actualContext.Action);
            Assert.AreEqual("Home", actualContext.Controller);
        }

       
        

    }
}
