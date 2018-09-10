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
    public class Given_I_Request_The_Report_Edit_Page : ReportControllerTestBase
    {
        [Test]
        public void The_Report_Exists_And_Is_Editable_Then_Show_Edit_Report()
        {
            // arrange
            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(CurrentValidNotSubmittedReport).Verifiable();
            _mockReportService.Setup(s => s.CanBeEdited(CurrentValidNotSubmittedReport)).Returns(true).Verifiable();

            // act
            var result = _controller.Edit();

            // assert
            _mockUrlHelper.VerifyAll();
            _mockReportService.VerifyAll();

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
        public void The_Report_Does_Not_Exist_Then_Should_Not_Error()
        {
            // arrange
            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns("report/create");
            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns((Report)null).Verifiable();
            _mockReportService.Setup(s => s.CanBeEdited(null)).Returns(false).Verifiable();

            // act
            var result = _controller.Edit();

            // assert
            _mockUrlHelper.VerifyAll();
            _mockReportService.VerifyAll();
            Assert.IsAssignableFrom<RedirectResult>(result);
        }

        [Test]
        public void The_Report_Exists_And_Not_Editable_Then_Redirect_Home()
        {
            // arrange
            const string url = "report/create";
            UrlActionContext actualContext = null;
            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");
            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(CurrentValidNotSubmittedReport).Verifiable();
            _mockReportService.Setup(s => s.CanBeEdited(CurrentValidNotSubmittedReport)).Returns(false).Verifiable();

            // act
            var result = _controller.Edit();

            // assert
            _mockUrlHelper.VerifyAll();
            _mockReportService.VerifyAll();

            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(url, redirectResult.Url);
            Assert.AreEqual("Index", actualContext.Action);
            Assert.AreEqual("Home", actualContext.Controller);
        }
    }
}