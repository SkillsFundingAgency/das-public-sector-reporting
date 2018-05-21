using System;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.ViewModels.Home;
using StructureMap;

namespace SFA.DAS.PSRService.IntegrationTests.Web
{
    [TestFixture]
    public class WhenUserEntersSite
    {
        private static Container _container;
        private HomeController _homeController;

        [SetUp]
        public void SetUp()
        {
            TestHelper.ClearData();
            _homeController = _container.GetInstance<HomeController>();
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _container = new Container();
            _container.Configure(TestHelper.ConfigureIoc());
        }


        [Test]
        public void AndThereAreNoReportsThenCreateReportIsEnabled()
        {
            // arrange

            // act
            var result = _homeController.Index() as ViewResult;
            
            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IndexViewModel>(result.Model);
            var model = (IndexViewModel) result.Model;
            Assert.IsTrue(model.CanCreateReport);
            Assert.IsFalse(model.CanEditReport);
        }

        [Test]
        public void AndThereIsSubmittedReportThenCreateReportIsDisabled()
        {
            // arrange
            TestHelper.CreateReport(new ReportDto
            {
                Id = Guid.NewGuid(),
                EmployerId = "111",
                Submitted = true,
                ReportingPeriod = TestHelper.CurrentPeriod.PeriodString,
                ReportingData = "{OrganisationName: '1'}"
            });

            // act
            var result = _homeController.Index() as ViewResult;
            
            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IndexViewModel>(result.Model);
            var model = (IndexViewModel) result.Model;
            Assert.IsFalse(model.CanCreateReport);
            Assert.IsFalse(model.CanEditReport);
        }

        [Test]
        public void AndThereIsUnsubmittedReportThenEditReportIsEnabled()
        {
            // arrange
            TestHelper.CreateReport(new ReportDto
            {
                Id = Guid.NewGuid(),
                EmployerId = "111",
                Submitted = false,
                ReportingPeriod = TestHelper.CurrentPeriod.PeriodString,
                ReportingData = "{OrganisationName: '1'}"
            });

            // act
            var result = _homeController.Index() as ViewResult;
            
            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IndexViewModel>(result.Model);
            var model = (IndexViewModel) result.Model;
            Assert.IsFalse(model.CanCreateReport);
            Assert.IsTrue(model.CanEditReport);
        }
    }
}
