using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests
{
    [TestFixture]
    public class Given_I_Request_The_Report_Confirm_Page : ReportControllerTestBase
    {
        [Test]
        public void When_The_Report_Is_Valid_To_Submit_Then_Show_Confirm_View()
        {
            // arrange
            var report =
                new ReportBuilder()
                    .WithValidSections()
                    .WithEmployerId("ABCDEF")
                    .ForCurrentPeriod()
                    .WhereReportIsNotAlreadySubmitted()
                    .Build();

            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(report).Verifiable();
            _mockReportService.Setup(s => s.CanBeEdited(report)).Returns(true).Verifiable();
            _controller.ObjectValidator = GetObjectValidator().Object;

            // act
            var result = _controller.Confirm();

            // assert
            Assert.IsAssignableFrom<ViewResult>(result);
            Assert.IsTrue(((ViewResult) result).ViewName == "Confirm" || ((ViewResult) result).ViewName == null);
            _mockReportService.VerifyAll();
        }

        [Test]
        public void When_Valid_Report_Confirmed_Then_Submit()
        {
            // arrange
            var report = new Report(); // not submitted and empty sections, should be valid for submission

            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(report).Verifiable();
            _mockReportService.Setup(s => s.SubmitReport(report)).Verifiable();
            _controller.ObjectValidator = GetObjectValidator().Object;

            // act
            var result = _controller.SubmitPost();

            // assert
            _mockReportService.VerifyAll();
            Assert.IsAssignableFrom<ViewResult>(result);
            Assert.AreEqual("SubmitConfirmation", ((ViewResult) result).ViewName);
        }

        private static Mock<IObjectModelValidator> GetObjectValidator()
        {
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<object>()));
            return objectValidator;
        }

        [Test]
        public void When_Unconfirmed_Report_Is_Not_Valid_To_Submit_Then_Redirect_To_Summary()
        {
            // arrange
            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(new Report()).Verifiable();
            _controller.ObjectValidator = GetFailingObjectValidator().Object;

            const string url = "report/create";
            UrlActionContext actualContext = null;
            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

            // act
            var result = _controller.Confirm();

            // assert
            _mockReportService.VerifyAll();
            Assert.IsAssignableFrom<RedirectResult>(result);
            Assert.IsNotNull(actualContext);
            Assert.AreEqual("Report", actualContext.Controller);
            Assert.AreEqual("Summary", actualContext.Action);
        }

        private static Mock<IObjectModelValidator> GetFailingObjectValidator()
        {
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<object>())).Callback<ActionContext, ValidationStateDictionary, string, object>((a, d, s, o) => { a.ModelState.AddModelError("1", "error"); });
            return objectValidator;
        }

        [Test]
        public void When_Confirmed_Report_Is_Not_Valid_To_Submit_Then_Redirect_To_Summary()
        {
            // arrange
            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(new Report()).Verifiable();

            _controller.ObjectValidator = GetFailingObjectValidator().Object;

            const string url = "report/create";
            UrlActionContext actualContext = null;
            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

            // act
            var result = _controller.SubmitPost();

            // assert
            _mockReportService.VerifyAll();
            Assert.IsAssignableFrom<RedirectResult>(result);
            Assert.IsNotNull(actualContext);
            Assert.AreEqual("Report", actualContext.Controller);
            Assert.AreEqual("Summary", actualContext.Action);
        }

        [Test]
        public void When_Unconfirmed_Report_Is_Not_Found_Then_Return_404()
        {
            // arrange
            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns((Report) null).Verifiable();

            // act
            var result = _controller.Confirm();

            // assert
            _mockReportService.VerifyAll();
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Test]
        public void When_Confirmed_Report_Is_Not_Found_Then_Return_404()
        {
            // arrange
            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), "ABCDE")).Returns((Report) null).Verifiable();

            // act
            var result = _controller.SubmitPost();

            // assert
            _mockReportService.VerifyAll();
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Test]
        public void When_The_Report_Is_Submitted_Redirect_To_Home()
        {
            // arrange
            var url = "home/Index";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");


            var report =
                new ReportBuilder()
                    .WithValidSections()
                    .WithEmployerId("ABCDEF")
                    .ForCurrentPeriod()
                    .WhereReportIsAlreadySubmitted()
                    .Build();
                
            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(report).Verifiable();
            _mockReportService.Setup(s => s.CanBeEdited(report)).Returns(false).Verifiable();
            _controller.ObjectValidator = GetObjectValidator().Object;

            // act
            var result = _controller.Confirm();

            // assert
            Assert.AreEqual(typeof(RedirectResult), result.GetType());
            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(url, redirectResult.Url);
            Assert.AreEqual(actualContext.Action, "Index");
            Assert.AreEqual(actualContext.Controller, actualContext.Controller);
        }

    }
}