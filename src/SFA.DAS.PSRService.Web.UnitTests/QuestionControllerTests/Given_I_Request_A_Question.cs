using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.UnitTests.QuestionControllerTests
{
    [TestFixture]
    public class Given_I_Request_A_Question
    {
        private QuestionController _controller;
        private Mock<IReportService> _reportService;
        private Mock<IEmployerAccountService> _EmployerAccountServiceMock;
        private Mock<IUrlHelper> _mockUrlHelper;
        private Mock<IPeriodService> _periodServiceMock;
        private EmployerIdentifier _employerIdentifier;

        [SetUp]
        public void SetUp()
        {
            _mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            _EmployerAccountServiceMock = new Mock<IEmployerAccountService>(MockBehavior.Strict);
            _reportService = new Mock<IReportService>(MockBehavior.Strict);
            _periodServiceMock = new Mock<IPeriodService>(MockBehavior.Strict);

            _periodServiceMock.Setup(s => s.GetCurrentPeriod()).Returns(new Period(DateTime.UtcNow));
            _periodServiceMock.Setup(s => s.IsSubmissionsOpen()).Returns(true);

            _controller = new QuestionController(_reportService.Object, _EmployerAccountServiceMock.Object, null,_periodServiceMock.Object) { Url = _mockUrlHelper.Object };
            
            _employerIdentifier = new EmployerIdentifier() { AccountId = "ABCDE", EmployerName = "EmployerName" };

            _EmployerAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(It.IsAny<HttpContext>())).Returns(_employerIdentifier);
            _EmployerAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(null)).Returns(_employerIdentifier);
            
        }

        [Test]
        public void And_A_Report_Doesnt_Exist_Then_Redirect_Home()
        {
            // arrange
            var url = "home/index";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");
            _reportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns((Report)null);
            _reportService.Setup(s => s.CanBeEdited(null)).Returns(false).Verifiable();

            // act
            var result = _controller.Index("YourEmployees");

            // assert
            _mockUrlHelper.VerifyAll();

            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(url, redirectResult.Url);
            Assert.AreEqual("Index", actualContext.Action);
            Assert.AreEqual("Home", actualContext.Controller);
        }

        [Test]
        public void And_A_Valid_Report_Doesnt_Exist_Then_Redirect_Home()
        {
            // arrange
            var url = "home/index";
            UrlActionContext actualContext = null;

            var report = ReportTestModelBuilder.CurrentReportWithInvalidSections("ABCDE");
            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");
            _reportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(report);
            _reportService.Setup(s => s.CanBeEdited(report)).Returns(false).Verifiable();

            // act
            var result = _controller.Index("YourEmployees");

            // assert
            _mockUrlHelper.VerifyAll();

            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(url, redirectResult.Url);
            Assert.AreEqual("Index", actualContext.Action);
            Assert.AreEqual("Home", actualContext.Controller);
        }

        [Test]
        public void And_The_Question_ID_Doesnt_Exist_Then_Return_Error()
        {
            // arrange
            var url = "home/index";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");
            _reportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(ReportTestModelBuilder.CurrentValidAndNotSubmittedReport("ABCDE"));
            _reportService.Setup(s => s.CanBeEdited(It.IsAny<Report>())).Returns(true).Verifiable();

            // act
            var result = _controller.Index("YourEmployees");
            
            // assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Test]
        public void The_Question_ID_Exists_And_Report_Is_Valid_Then_Show_Question_Page()
        {
            

            var url = "home/index";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");
            _reportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(ReportTestModelBuilder.CurrentValidAndNotSubmittedReport("ABCDE"));
            _reportService.Setup(s => s.CanBeEdited(It.IsAny<Report>())).Returns(true).Verifiable();

            // act
            var result = _controller.Index("SectionOne");

            // assert
            Assert.AreEqual(typeof(ViewResult), result.GetType());
            var listViewResult = result as ViewResult;
            Assert.IsNotNull(listViewResult);
            Assert.AreEqual("Index", listViewResult.ViewName, "View name does not match, should be: Index");


            var sectionViewModel = listViewResult.Model as SectionViewModel;
            Assert.IsNotNull(sectionViewModel);

            var report = sectionViewModel.Report;
            Assert.IsNotNull(report);

            var sectionOne = ReportTestModelBuilder.SectionOne;
            var questionSection = sectionViewModel.CurrentSection;
            Assert.IsNotNull(questionSection);
            Assert.AreEqual(questionSection.Id, sectionOne.Id);

            

            CollectionAssert.AreEqual(questionSection.Questions, sectionOne.Questions);
        }
    }
}
