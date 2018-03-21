using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.Controllers;
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


        [SetUp]
        public void SetUp()
        {
            _mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
         _EmployerAccountServiceMock = new Mock<IEmployerAccountService>(MockBehavior.Strict);
            _reportService = new Mock<IReportService>(MockBehavior.Strict);
            _controller = new QuestionController(_reportService.Object) { Url = _mockUrlHelper.Object };
            _reportService.Setup(s => s.GetCurrentReportPeriod()).Returns("1617");
        }

        [Test]
        public void And_A_Report_Doesnt_Exist_Then_Redirect_Home()
        {
            // arrange
            var url = "home/index";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");
            _reportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<long>())).Returns((Report)null);

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

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");
            _reportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<long>())).Returns(new Report(){Submitted = false});
            _reportService.Setup(s => s.IsSubmitValid(It.IsAny<Report>())).Returns(false);
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

            _reportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<long>())).Returns(new Report() { Submitted = true });
            _reportService.Setup(s => s.IsSubmitValid(It.IsAny<Report>())).Returns(true);
            _reportService.Setup(s => s.GetQuestionSection(It.IsAny<string>(), It.IsAny<Report>())).Returns((Section)null);

            // act
            var result = _controller.Index("YourEmployees");

         
            Assert.AreEqual(result.GetType(),typeof(BadRequestResult));
            
        }

        [Test]
        public void And_The_Question_ID_Exists_More_Than_Once_Then_Return_Error()
        {
            // arrange
            var url = "home/index";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

            _reportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<long>())).Returns(new Report() { Submitted = true });
            _reportService.Setup(s => s.IsSubmitValid(It.IsAny<Report>())).Returns(true);
            _reportService.Setup(s => s.GetQuestionSection(It.IsAny<string>(), It.IsAny<Report>()))
                .Throws(new Exception());

            // act

            Assert.Throws<Exception>(() => _controller.Index("YourEmployees"));
            


//            Assert.AreEqual(result.GetType(), typeof(BadRequestResult));

        }

        [Test]
        public void The_Question_ID_Exists_And_Report_Is_Valid_Then_Show_Question_Page()
        {
            // arrange
            // arrange
            var Questions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "0",
                    Type = QuestionType.Number,
                    Optional = false
                }
                ,new Question()
                {
                    Id = "atEnd",
                    Answer = "0",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "newThisPeriod",
                    Answer = "0",
                    Type = QuestionType.Number,
                    Optional = false
                }

            };

            var SectionOne = new Section()
            {
                Id = "SectionOne",
                SubSections = new List<Section>() { new Section{
                    Id = "SubSectionOne",
                    Questions = Questions,
                    Title = "SubSectionOne",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionOne"
            };

            var url = "home/index";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");
            _reportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<long>())).Returns(new Report() { Submitted = false });
            _reportService.Setup(s => s.IsSubmitValid(It.IsAny<Report>())).Returns(true);

            _reportService.Setup(s => s.GetQuestionSection(It.IsAny<string>(), It.IsAny<Report>())).Returns(SectionOne);
            // act
            var result = _controller.Index("YourEmployees");

            // assert
            Assert.AreEqual(typeof(ViewResult), result.GetType());
            var listViewResult = result as ViewResult;
            Assert.IsNotNull(listViewResult);
            Assert.AreEqual("Index", listViewResult.ViewName, "View name does not match, should be: Index");


            var questionViewModel = listViewResult.Model as QuestionViewModel;
            Assert.IsNotNull(questionViewModel);

            var report = questionViewModel.Report;
            Assert.IsNotNull(report);

            var questionSection = questionViewModel.CurrentSection;
            Assert.IsNotNull(questionSection);
            Assert.AreEqual(questionSection.Id,SectionOne.Id);
            CollectionAssert.AreEqual(questionSection.Questions,SectionOne.Questions);
        }
    }
}
