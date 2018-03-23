using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Models.Home;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests
{
    [TestFixture]
    public class WhenHomePageDisplayed
    {
        private HomeController _controller;
        private Mock<IReportService> _mockReportService;
        private Mock<IEmployerAccountService> _employeeAccountServiceMock;

        [SetUp]
        public void SetUp()
        {
            _mockReportService = new Mock<IReportService>(MockBehavior.Strict);
            _employeeAccountServiceMock = new Mock<IEmployerAccountService>(MockBehavior.Strict);
            _controller = new HomeController(_mockReportService.Object, _employeeAccountServiceMock.Object);

            _mockReportService.Setup(s => s.GetCurrentReportPeriod()).Returns("1617");
            _employeeAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(It.IsAny<HttpContext>()))
                .Returns("ABCDE");
            _employeeAccountServiceMock.Setup(s => s.GetCurrentEmployerAccountId(null))
                .Returns("ABCDE");
        }

        [Test]
        public void AndThereIsNoCurrentReportThenCreateReportEnabled()
        {
            // arrange
            var period = "1617";
            var periodName = "whatever the name";

            _mockReportService.Setup(r => r.GetCurrentReportPeriod()).Returns(period).Verifiable("Current period wasn't requested");
            _mockReportService.Setup(r => r.GetCurrentReportPeriodName(period)).Returns(periodName).Verifiable("Current period name wasn't requested");
            _mockReportService.Setup(r => r.GetReport(period, "ABCDE")).Returns((Report)null).Verifiable("Current report wasn't requested");

            // act
            var result = _controller.Index();

            // assert
            _mockReportService.VerifyAll();

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as IndexViewModel;
            Assert.IsNotNull(model);
            Assert.IsTrue(model.CanCreateReport);
            Assert.IsFalse(model.CanEditReport);
            Assert.AreEqual(periodName, model.PeriodName);
        }

        [Test]
        public void AndThereIsCurrentReportThenEditReportEnabled()
        {
            // arrange
            var period = "1617";
            var periodName = "whatever the name";
            var report = new Report();

            _mockReportService.Setup(r => r.GetCurrentReportPeriod()).Returns(period).Verifiable("Current period wasn't requested");
            _mockReportService.Setup(r => r.GetCurrentReportPeriodName(period)).Returns(periodName).Verifiable("Current period name wasn't requested");
            _mockReportService.Setup(r => r.GetReport(period, "ABCDE")).Returns(report).Verifiable("Current report wasn't requested");

            // act
            var result = _controller.Index();

            // assert
            _mockReportService.VerifyAll();

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as IndexViewModel;
            Assert.IsNotNull(model);
            Assert.IsFalse(model.CanCreateReport);
            Assert.IsTrue(model.CanEditReport);
            Assert.AreEqual(periodName, model.PeriodName);
        }

        [Test]
        public void AndThereIsSubmittedCurrentReportThenEditReportEnabled()
        {
            // arrange
            var period = "1617";
            var periodName = "whatever the name";
            var report = new Report {Submitted = true};

            _mockReportService.Setup(r => r.GetCurrentReportPeriod()).Returns(period).Verifiable("Current period wasn't requested");
            _mockReportService.Setup(r => r.GetCurrentReportPeriodName(period)).Returns(periodName).Verifiable("Current period name wasn't requested");
            _mockReportService.Setup(r => r.GetReport(period, "ABCDE")).Returns(report).Verifiable("Current report wasn't requested");

            // act
            var result = _controller.Index();

            // assert
            _mockReportService.VerifyAll();

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as IndexViewModel;
            Assert.IsNotNull(model);
            Assert.IsFalse(model.CanCreateReport);
            Assert.IsFalse(model.CanEditReport);
            Assert.AreEqual(periodName, model.PeriodName);
        }
    }
}
