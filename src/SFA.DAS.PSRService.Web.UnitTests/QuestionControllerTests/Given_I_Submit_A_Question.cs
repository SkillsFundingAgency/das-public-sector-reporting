using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.UnitTests.QuestionControllerTests
{
    [TestFixture]
    public class Given_I_Submit_A_Question
    {
        private QuestionController _controller;
        private Mock<IReportService> _reportService;
        private Mock<IEmployerAccountService> _employerAccountServiceMock;
        private Mock<IUrlHelper> _mockUrlHelper;
        private Mock<IUserService> _mockUserService;
        //private SectionModel _sectionModel;

        private EmployerIdentifier _employerIdentifier;
        private Mock<IPeriodService> _periodServiceMock;

        [SetUp]
        public void SetUp()
        {
            _mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            _employerAccountServiceMock = new Mock<IEmployerAccountService>(MockBehavior.Strict);
            _reportService = new Mock<IReportService>(MockBehavior.Strict);
            _periodServiceMock = new Mock<IPeriodService>(MockBehavior.Strict);
            _mockUserService = new Mock<IUserService>(MockBehavior.Strict);

            
            _employerIdentifier = new EmployerIdentifier() { AccountId = "ABCDE", EmployerName = "EmployerName" };

            _employerAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(It.IsAny<HttpContext>()))
                .Returns(_employerIdentifier);
            _employerAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(null))
                .Returns(_employerIdentifier);

            _controller = new QuestionController(_reportService.Object, _employerAccountServiceMock.Object, null, _periodServiceMock.Object, _mockUserService.Object) { Url = _mockUrlHelper.Object };

            //_sectionModel = new SectionModel
            //{
            //    Report = ReportTestModelBuilder.CurrentReportWithValidSections("ABCDE"),
            //    CurrentSection = ReportTestModelBuilder.SectionOne.SubSections.FirstOrDefault()
            //};

        }

        [Test]
        public void And_A_Report_Doesnt_Exist_Then_Redirect_Home()
        {
            // arrange
            var url = "home/index";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");
            _reportService.Setup(s => s.GetReport("111", It.IsAny<string>())).Returns((Report)null).Verifiable();
            _reportService.Setup(s => s.CanBeEdited(null)).Returns(false).Verifiable();

            // act
            var result = _controller.Submit(new SectionModel{ReportingPeriod = "111"});

            // assert
            _mockUrlHelper.VerifyAll();
            _reportService.VerifyAll();

            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(url, redirectResult.Url);
            Assert.AreEqual("Index", actualContext.Action);
            Assert.AreEqual("Home", actualContext.Controller);
        }

        [Test]
        public void And_Report_Is_Not_Editable_Then_Redirect_Home()
        {
            // arrange
            var url = "home/index";
            UrlActionContext actualContext = null;
            var report = new Report();

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");
            _reportService.Setup(s => s.GetReport("111", It.IsAny<string>())).Returns(report).Verifiable();
            _reportService.Setup(s => s.CanBeEdited(report)).Returns(false).Verifiable();

            // act
            var result = _controller.Submit(new SectionModel{ReportingPeriod = "111"});

            // assert
            _mockUrlHelper.VerifyAll();
            _reportService.VerifyAll();

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
            _reportService.Setup(s => s.GetReport("111", It.IsAny<string>())).Returns(ReportTestModelBuilder.CurrentReportWithValidSections("ABCDE")).Verifiable();
            _reportService.Setup(s => s.CanBeEdited(It.IsAny<Report>())).Returns(true).Verifiable();

            // act
            var result = _controller.Submit(new SectionModel{ReportingPeriod = "111", Id = "No such section"});

            // assert
            _reportService.VerifyAll();
            Assert.IsAssignableFrom<BadRequestResult>(result);
        }

        [Test]
        public void The_SectionViewModel_Is_Valid_Then_Save_Question_Section()
        {
            Report actualReport = null;
            var url = "home/index";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");
            _reportService.Setup(s => s.GetReport("222", It.IsAny<string>())).Returns(ReportTestModelBuilder.CurrentReportWithValidSections("ABCDE")).Verifiable();
            _reportService.Setup(s => s.SaveReport(It.IsAny<Report>(), It.IsAny<UserModel>())).Callback<Report, UserModel>((r, u) => actualReport = r).Verifiable("Report was not saved");
            _reportService.Setup(s => s.CanBeEdited(It.IsAny<Report>())).Returns(true);
            _mockUserService.Setup(s => s.GetUserModel(It.IsAny<ClaimsPrincipal>())).Returns(new UserModel()).Verifiable();

            var sectionModel = new SectionModel
            {
                Id = "SubSectionOne",
                ReportingPeriod = "222",
                Questions = new []
                {
                    new QuestionViewModel
                    {
                        Id = "atEnd",
                        Answer = "123,000"
                    },
                    new QuestionViewModel
                    {
                        Id = "atStart",
                        Answer = "123"
                    }
                }
            };

            // act
            var result = _controller.Submit(sectionModel);

            // assert
            _reportService.VerifyAll();
            _mockUserService.VerifyAll();

            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(url, redirectResult.Url);
            Assert.AreEqual("Edit", actualContext.Action);
            Assert.AreEqual("Report", actualContext.Controller);

            Assert.IsNotNull(actualReport);
            var section = actualReport.GetQuestionSection("SubSectionOne");
            Assert.IsNotNull(section);
            Assert.AreEqual(3, section.Questions.Count());
            Assert.AreEqual("123", section.Questions.Single(q => q.Id == "atStart").Answer);
            Assert.AreEqual("123000", section.Questions.Single(q => q.Id == "atEnd").Answer);
            Assert.AreEqual("1,000", section.Questions.Single(q => q.Id == "newThisPeriod").Answer);
        }

        [Test]
        public void The_SectionViewModel_Is_Valid_But_Report_Is_Not_Full_Then_Save_Question_Section()
        {
            // arrange
            Report actualReport = null;
            var report = ReportTestModelBuilder.CurrentReportWithValidSections("123");
            var url = "home/index";
            UrlActionContext actualContext = null;
            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");
            _reportService.Setup(s => s.GetReport("222", It.IsAny<string>())).Returns(report).Verifiable();
            _reportService.Setup(s => s.SaveReport(It.IsAny<Report>(), It.IsAny<UserModel>())).Callback<Report, UserModel>((r, u) => actualReport = r).Verifiable("Report was not saved");
            _reportService.Setup(s => s.CanBeEdited(report)).Returns(true).Verifiable();
            _mockUserService.Setup(s => s.GetUserModel(It.IsAny<ClaimsPrincipal>())).Returns(new UserModel()).Verifiable();

            var sectionModel = new SectionModel
            {
                Id = "SubSectionOne",
                ReportingPeriod = "222",
                Questions = new []
                {
                    new QuestionViewModel
                    {
                        Id = "atEnd",
                        Answer = "123,000"
                    },
                    new QuestionViewModel
                    {
                        Id = "atStart",
                        Answer = ""
                    }
                }
            };            // act
            var result = _controller.Submit(sectionModel);

            // assert
            _mockUrlHelper.VerifyAll();
            _reportService.VerifyAll();
            _mockUserService.VerifyAll();

            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(url, redirectResult.Url);
            Assert.AreEqual("Edit", actualContext.Action);
            Assert.AreEqual("Report", actualContext.Controller);

            Assert.IsNotNull(actualReport);
            Assert.IsFalse(actualReport.IsValidForSubmission());
        }
    }
}
