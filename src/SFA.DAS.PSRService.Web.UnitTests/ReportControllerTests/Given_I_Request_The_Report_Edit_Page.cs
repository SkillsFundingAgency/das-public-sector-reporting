using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests
{
    [TestFixture]
    public class Given_I_Request_The_Report_Edit_Page : ReportControllerTestBase
    {

        // Period is null tests
        [Test]
        public void The_Report_Exists_And_Current_Period_And_It_Hasnt_Been_Submitted_Then_Show_Edit_Report()
        {
            // arrange
            _mockReportService.Setup(s => s.IsSubmitValid(It.IsAny<Report>())).Returns(true);
            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(_reportList.FirstOrDefault());
            // act
            var result = _controller.Edit("1718");

            // assert
            _mockUrlHelper.VerifyAll();

            // assert
            Assert.AreEqual(typeof(ViewResult), result.GetType());
            var editViewResult = result as ViewResult;
            Assert.IsNotNull(editViewResult);
            Assert.AreEqual("Edit", editViewResult.ViewName, "View name does not match, should be: List");


            Assert.AreEqual(editViewResult.Model.GetType(), typeof(ReportViewModel));
            var reportViewModel = editViewResult.Model as ReportViewModel;
            Assert.IsNotNull(reportViewModel);
            Assert.IsNotNull(reportViewModel.Report.Id);
           
     

        }

        [Test]
        [TestCase("1617",1234)]
        public void The_Report_Exists_And_Previous_Period_And_It_Hasnt_Been_Submitted_Then_Redirect_Home(string previousPeriod, long employerId)
        {
            // arrange
            var url = "report/create";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url)
                .Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

            _mockReportService.Setup(s => s.IsSubmitValid(It.IsAny<Report>())).Returns(false);
            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(_reportList.FirstOrDefault(w => w.ReportingPeriod == previousPeriod));
            // act
            var result = _controller.Edit(previousPeriod);

            // assert
            _mockUrlHelper.VerifyAll();

            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(url, redirectResult.Url);
            Assert.AreEqual("Index", actualContext.Action);
            Assert.AreEqual("Home", actualContext.Controller);
        }

        [Test]
        public void The_Report_Exists_And_It_Has_Been_Submitted_Then_Redirect_To_Home()
        {
            // arrange
            var url = "report/create";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url)
                .Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

            _mockReportService.Setup(s => s.IsSubmitValid(It.IsAny<Report>())).Returns(false);
            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(new Report()
            {
                Submitted = true
            });
            // act
            var result = _controller.Edit("1617");

            // assert
            _mockUrlHelper.VerifyAll();

            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(url, redirectResult.Url);
            Assert.AreEqual("Index", actualContext.Action);
            Assert.AreEqual("Home", actualContext.Controller);
        }

        [Test]
        public void The_Report_Doesnt_Exist_Then_Redirect_To_Home()
        {
            // arrange
            // arrange
            var url = "report/create";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url)
                .Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

            _mockReportService.Setup(s => s.IsSubmitValid(It.IsAny<Report>())).Returns(false);
            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns((Report)null);
           
            // act
            var result = _controller.Edit("1617");

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