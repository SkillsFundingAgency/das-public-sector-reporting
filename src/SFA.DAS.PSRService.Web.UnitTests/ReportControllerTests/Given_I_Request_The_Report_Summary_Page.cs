using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests
{
    [TestFixture]
    public class Given_I_Request_The_Summary_Page : ReportControllerTestBase
    {
        
        [Test]
     
        public void And_The_Report_Exists_And_Is_Valid_Then_Show_Summary_Page()
        {
            var ApprenticeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "20",
                    Type = QuestionType.Number,
                    Optional = false
                }
                ,new Question()
                {
                    Id = "atEnd",
                    Answer = "35",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "newThisPeriod",
                    Answer = "18",
                    Type = QuestionType.Number,
                    Optional = false
                }

            };
            var EmployeeQuestions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "250",
                    Type = QuestionType.Number,
                    Optional = false
                }
                ,new Question()
                {
                    Id = "atEnd",
                    Answer = "300",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "newThisPeriod",
                    Answer = "50",
                    Type = QuestionType.Number,
                    Optional = false
                }

            };
            var YourEmployees = new Section()
            {
                Id = "YourEmployeesSection",
                SubSections = new List<Section>() { new Section{
                    Id = "YourEmployees",
                    Questions = EmployeeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionTwo"
            };

            var YourApprentices = new Section()
            {
                Id = "YourApprenticeSection",
                SubSections = new List<Section>() { new Section{
                    Id = "YourApprentices",
                    Questions = ApprenticeQuestions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionTwo"
            };


            IList<Section> sections = new List<Section>();

            sections.Add(YourEmployees);
            sections.Add(YourApprentices);
            var report = new Report()
            {
                ReportingPeriod = "1617",
                Sections = sections,
                SubmittedDetails = new Submitted(),
                Submitted = true
            };

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));
            _controller.ObjectValidator = objectValidator.Object;

            // arrange
            _mockReportService.Setup(s => s.IsSubmitValid(It.IsAny<Report>())).Returns(true);
            _mockReportService.Setup(s => s.GetCurrentReportPeriod()).Returns("1617");
            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(report);
            _mockReportService.Setup(s => s.CalculatePercentages(It.IsAny<Report>()))
                .Returns(new ReportingPercentages());
            _mockReportService.Setup(s => s.GetPeriod(It.IsAny<string>())).Returns(new CurrentPeriod());
            // act
            var result = _controller.Summary("1718");


            // assert
            Assert.AreEqual(typeof(ViewResult), result.GetType());
            var editViewResult = result as ViewResult;
            Assert.IsNotNull(editViewResult);
            Assert.AreEqual("Summary", editViewResult.ViewName, "View name does not match, should be: Summary");


            Assert.AreEqual(editViewResult.Model.GetType(), typeof(ReportViewModel));
            var reportViewModel = editViewResult.Model as ReportViewModel;
            Assert.IsNotNull(reportViewModel);
            var reportResult = reportViewModel.Report;
            Assert.IsNotNull(reportResult);
            Assert.IsNotNull(reportResult.Id);
        }

        [Test]
        public void And_Report_Doesnt_Exist_Then_Redirect_To_Home()
        {
            // arrange
            var url = "Home/Index";
            UrlActionContext actualContext = null;

            _mockReportService.Setup(s => s.GetCurrentReportPeriod()).Returns("1617");
            _mockReportService.Setup(s => s.IsSubmitValid(It.IsAny<Report>())).Returns(true);
            _mockReportService.Setup(s => s.CalculatePercentages(It.IsAny<Report>()))
                .Returns(new ReportingPercentages());
            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");
            _mockReportService.Setup(s => s.GetPeriod(It.IsAny<string>())).Returns(new CurrentPeriod());
            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns((Report) null);
            // act
            var result = _controller.Summary("NoReport");

            // assert
            _mockUrlHelper.VerifyAll();

            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(url, redirectResult.Url);
            Assert.AreEqual("Index", actualContext.Action);
            Assert.AreEqual("Home", actualContext.Controller);
        }

        [Test]
        [Ignore("Obsolete")]
        public void And_The_Period_Is_Null_Then_Redirect_To_Home()
        {
            // arrange
            var url = "Home/Index";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns((Report)null);
            // act
            var result = _controller.Summary(null);

            // assert
            _mockUrlHelper.VerifyAll();

            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(url, redirectResult.Url);
            Assert.AreEqual("Index", actualContext.Action);
            Assert.AreEqual("Home", actualContext.Controller);
        }

        [Test]
        public void And_The_Report_Service_Throws_An_Exception()
        {
            // arrange
            var url = "Home/Index";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception("get report Error"));
            // act
            var result = _controller.Summary("ReportError");

            // assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }


    }
}
