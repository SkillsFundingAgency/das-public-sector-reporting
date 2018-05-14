using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Models.Home;
using SFA.DAS.PSRService.Web.ViewModels;
using StructureMap;

namespace SFA.DAS.PSRService.IntegrationTests.Web
{
    [TestFixture]
    public class WhenUserCreatesReport
    {
        private static Container _container;
        private ReportController _reportController;
        private QuestionController _questionController;
        private Mock<IUrlHelper> _mockUrlHelper;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _container = new Container();
            _container.Configure(TestHelper.ConfigureIoc());
            _mockUrlHelper = new Mock<IUrlHelper>();
            _mockUrlHelper.Setup(u => u.Action(It.IsAny<UrlActionContext>())).Returns("!");
        }

        [SetUp]
        public void SetUp()
        {
            TestHelper.ClearData();
            _reportController = _container.GetInstance<ReportController>();
            _reportController.Url = _mockUrlHelper.Object;

            _questionController = _container.GetInstance<QuestionController>();
            _questionController.Url = _mockUrlHelper.Object;
        }

        [Test]
        public void AndReportIsSubmittedThenItShouldAppearInSubmittedReportList()
        {
            // arrange
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(), 
                It.IsAny<ValidationStateDictionary>(), 
                It.IsAny<string>(), 
                It.IsAny<Object>()));
            _reportController.ObjectValidator = objectValidator.Object;

            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(c => c.User).Returns(new TestPrincipal());
            _reportController.ControllerContext.HttpContext = mockContext.Object;

            // act
            _reportController.PostCreate();
            _questionController.Submit(new SectionViewModel
            {
                Report = new Report {EmployerId = "111", ReportingPeriod = TestHelper.CurrentPeriod.PeriodString},
                CurrentSection = new Section
                {
                    Id = "YourEmployees"
                },
                Questions = new QuestionViewModel[]
                {
                    new QuestionViewModel {Id = "atStart", Answer = "1"},
                    new QuestionViewModel {Id = "atEnd", Answer = "1"},
                    new QuestionViewModel {Id = "newThisPeriod", Answer = "1"}
                }
            });
            
            _questionController.Submit(new SectionViewModel
            {
                Report = new Report {EmployerId = "111", ReportingPeriod = TestHelper.CurrentPeriod.PeriodString},
                CurrentSection = new Section
                {
                    Id = "YourApprentices"
                },
                Questions = new QuestionViewModel[]
                {
                    new QuestionViewModel {Id = "atStart", Answer = "1"},
                    new QuestionViewModel {Id = "atEnd", Answer = "1"},
                    new QuestionViewModel {Id = "newThisPeriod", Answer = "1"}
                }
            });

            _reportController.Submit(TestHelper.CurrentPeriod.PeriodString);
            var result = _reportController.List() as ViewResult;

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ReportListViewModel>(result.Model);
            var model = (ReportListViewModel) result.Model;
            Assert.AreEqual(1, model.SubmittedReports.Count());
        }
    }
}
