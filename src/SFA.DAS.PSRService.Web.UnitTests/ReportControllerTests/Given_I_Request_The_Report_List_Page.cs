using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests
{
    [TestFixture]
    public class Given_I_Request_The_Report_List_Page :ReportControllerTestBase
    {
 

        [Test]
        public void And_More_Than_One_Report_Then_Show_List()
        {
            // arrange
           _mockReportService.Setup(s => s.GetSubmittedReports(It.IsAny<string>())).Returns(ReportList);
           // _mockReportService.Setup(s => s.GetPeriod(It.IsAny<string>())).Returns(new CurrentPeriod());
            // act
            var result = _controller.List();

            // assert
            Assert.AreEqual(typeof(ViewResult), result.GetType());
            var listViewResult = result as ViewResult;
            Assert.IsNotNull(listViewResult);
            Assert.AreEqual("List", listViewResult.ViewName,"View name does not match, should be: List");
            
            
            var listViewModel = listViewResult.Model as ReportListViewModel;
            Assert.IsNotNull(listViewModel);

            var reportList = listViewModel.SubmittedReports;
            Assert.IsNotEmpty(reportList);
            CollectionAssert.AllItemsAreInstancesOfType(reportList, typeof(Report));
            CollectionAssert.AreEqual(reportList, ReportList);
      
       
        }
        [Test]
        [Ignore("No longer a requirement")]
        public void And_Only_One_Report_Then_Redirect_To_Edit()
        {
            // arrange
            var url = "report/edit";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

            _mockReportService.Setup(s => s.GetSubmittedReports(It.IsAny<string>())).Returns(ReportList.Take(1).ToList);
            // act
            var result = _controller.List();

            // assert
            _mockUrlHelper.VerifyAll();

            Assert.AreEqual(typeof(RedirectResult), result.GetType());
            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(url, redirectResult.Url);
            Assert.AreEqual("Edit", actualContext.Action);
            Assert.Null(actualContext.Controller);
        }
        [Test]
        [Ignore("No longer a requirement")]
        public void And_No_Report_Then_Redirect_To_Start()
        {
            var url = "home/index";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

            _mockReportService.Setup(s => s.GetSubmittedReports(It.IsAny<string>())).Returns(new List<Report>());
            // act
            var result = _controller.List();

            // assert
            Assert.AreEqual(typeof(RedirectResult), result.GetType());
            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(url, redirectResult.Url);
            Assert.AreEqual("Index", actualContext.Action);
            Assert.AreEqual("Home", actualContext.Controller);
        }


    }
}
