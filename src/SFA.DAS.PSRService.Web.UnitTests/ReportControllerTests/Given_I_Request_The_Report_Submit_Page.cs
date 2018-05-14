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

            _mockReportService.Setup(s => s.SubmitReport(It.IsAny<Report>())).Returns(SubmittedStatus.Submitted);
          //  _mockReportService.Setup(s => s.GetPeriod(It.IsAny<string>())).Returns(new CurrentPeriod());
            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(new Report());

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));
            _controller.ObjectValidator = objectValidator.Object;
            // act
            var result = _controller.Submit();

            // assert
            Assert.AreEqual(typeof(ViewResult), result.GetType());
            var editViewResult = result as ViewResult;
            Assert.IsNotNull(editViewResult);
            Assert.AreEqual("Submitted", editViewResult.ViewName, "View name does not match, should be: List");


        }

        [Test]
      
        public void The_Report_Is_Not_Valid_To_Submit_Then_Redirect_Home()
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
            var url = "home/Index";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url)
                .Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

           
            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(report);
            _mockReportService.Setup(s => s.SubmitReport(report)).Returns(SubmittedStatus.Invalid);
            
            // act
            var result = _controller.Submit();

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
