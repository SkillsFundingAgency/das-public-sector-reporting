using System;
using System.Collections.Generic;
using System.Linq;
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
    public class Given_I_Submit_A_Question
    {
        private QuestionController _controller;
        private Mock<IReportService> _reportService;
        private Mock<IEmployerAccountService> _EmployerAccountServiceMock;
        private Mock<IUrlHelper> _mockUrlHelper;
        private SectionViewModel _sections;

        private EmployerIdentifier _employerIdentifier;
        private Mock<IPeriodService> _periodServiceMock;

        [SetUp]
        public void SetUp()
        {
            _mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            _EmployerAccountServiceMock = new Mock<IEmployerAccountService>(MockBehavior.Strict);
            _reportService = new Mock<IReportService>(MockBehavior.Strict);
            _periodServiceMock = new Mock<IPeriodService>(MockBehavior.Strict);
            
            
            _employerIdentifier = new EmployerIdentifier() { AccountId = "ABCDE", EmployerName = "EmployerName" };

            _EmployerAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(It.IsAny<HttpContext>()))
                .Returns(_employerIdentifier);
            _EmployerAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(null))
                .Returns(_employerIdentifier);

            _controller = new QuestionController(_reportService.Object, _EmployerAccountServiceMock.Object, null, _periodServiceMock.Object) { Url = _mockUrlHelper.Object };

            _sections = new SectionViewModel()
            {
                Report = ReportTestModelBuilder.CurrentReportWithValidSections("ABCDE"),
                CurrentSection = ReportTestModelBuilder.SectionOne.SubSections.FirstOrDefault()
            };

        }

        [Test]
        public void And_A_Report_Doesnt_Exist_Then_Redirect_Home()
        {
            // arrange
            var url = "home/index";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");
            _reportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns((Report)null);

            // act
            var result = _controller.Submit(_sections);

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
            _reportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(ReportTestModelBuilder.CurrentReportWithInvalidSections("ABCDE"));


            // act
            var result = _controller.Submit(_sections);

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
            var invalidSection = new SectionViewModel()
            {
                Report = ReportTestModelBuilder.CurrentReportWithValidSections("ABCDE"),
                CurrentSection = ReportTestModelBuilder.SectionTwo.SubSections.FirstOrDefault()
            };

            // arrange
            var url = "home/index";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

            _reportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(ReportTestModelBuilder.CurrentReportWithValidSections("ABCDE"));


            // act
            Assert.Throws<InvalidOperationException>(() => _controller.Submit(invalidSection));

        }

        [Test]
        [Ignore("Re-enable after test refactor")]
        public void And_The_Question_ID_Exists_More_Than_Once_Then_Return_Error()
        {
            // arrange
            var url = "home/index";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");
            _reportService.Setup(s => s.GetReport(It.IsAny<string>(), "ABCDE"))
                .Returns(ReportTestModelBuilder.CurrentReportWithDuplicateSections("ABCDE"));

            var duplicateSections = new SectionViewModel()
            {
                Report = ReportTestModelBuilder.CurrentReportWithDuplicateSections("ABCDE"),
                CurrentSection = ReportTestModelBuilder.SectionOne.SubSections.FirstOrDefault()
            };
            // act
            Assert.Throws<Exception>(() => _controller.Submit(duplicateSections));
        }

        [Test]
        public void The_SectionViewModel_Is_Valid_Then_Save_Question_Section()
        {


            var url = "home/index";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");
            _reportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(ReportTestModelBuilder.CurrentReportWithValidSections("ABCDE"));

            _reportService.Setup(s => s.SaveReport(It.IsAny<Report>()));

            // act
            _sections.Questions = _sections.CurrentSection.Questions.Select(s => new QuestionViewModel() { Answer = s.Answer, Id = s.Id, Optional = s.Optional, Type = s.Type }).ToList();
            var result = _controller.Submit(_sections);

            // assert
            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(url, redirectResult.Url);
            Assert.AreEqual("Edit", actualContext.Action);
            Assert.AreEqual("Report", actualContext.Controller);
        }


    }
}
