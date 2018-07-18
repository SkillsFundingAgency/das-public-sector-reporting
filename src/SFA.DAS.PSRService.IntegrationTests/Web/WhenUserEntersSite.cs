using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
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

            //Replace authorization service with one tailored to this test
            //TODO: Refactor test to Geiven-Then-When style and find a better way of onfiguring authorization
            _container.Inject<IAuthorizationService>(BuildMockAuthorizationService(new[] { PolicyNames.CanEditReport, PolicyNames.CanSubmitReport }));
        }

        private static IAuthorizationService BuildMockAuthorizationService(IEnumerable<string> policyNames)
        {
            var service = new Mock<IAuthorizationService>();
            service
                .Setup(
                    m => m.AuthorizeAsync(
                        It.IsAny<ClaimsPrincipal>(),
                        It.IsAny<object>(),
                        It.Is<string>(x => policyNames.Contains(x))))
                .Returns(
                    Task.FromResult(AuthorizationResult.Success()));

            return service.Object;
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
