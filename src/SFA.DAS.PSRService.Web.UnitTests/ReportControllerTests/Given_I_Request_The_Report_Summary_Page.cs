﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Controllers;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests
{
    [TestFixture]
    public class Given_I_Request_The_Summary_Page : ReportControllerTestBase
    {
        
        [Test]
        public void And_The_Report_Exists_Then_Show_Summary_Page()
        {
            // arrange

            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<long>())).Returns(new Report());
            // act
            var result = _controller.Summary("1718");


            // assert
            Assert.AreEqual(typeof(ViewResult), result.GetType());
            var editViewResult = result as ViewResult;
            Assert.IsNotNull(editViewResult);
            Assert.AreEqual("Summary", editViewResult.ViewName, "View name does not match, should be: Summary");


            Assert.AreEqual(editViewResult.Model.GetType(), typeof(Report));
            var reportViewModel = editViewResult.Model as Report;
            Assert.IsNotNull(reportViewModel);
            Assert.IsNotNull(reportViewModel.Id);
        }

        [Test]
        public void And_Report_Doesnt_Exist_Then_Redirect_To_Home()
        {
            // arrange
            var url = "Home/Index";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<long>())).Returns((Report) null);
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
        public void And_The_Period_Is_Null_Then_Redirect_To_Home()
        {
            // arrange
            var url = "Home/Index";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<long>())).Returns((Report)null);
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

            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<long>())).Throws(new Exception("get report Error"));
            // act
            var result = _controller.Summary("ReportError");

            // assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }


    }
}