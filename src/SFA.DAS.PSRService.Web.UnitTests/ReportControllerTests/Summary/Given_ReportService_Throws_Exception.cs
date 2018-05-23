using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Summary
{
    [ExcludeFromCodeCoverage]
    public class Given_ReportService_Throws_Exception
        : Given_A_ReportController
    {
        public Given_ReportService_Throws_Exception()
        {
            var url = "Home/Index";
            UrlActionContext actualContext = null;

            _mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception("get report Error"));
        }

        [ExcludeFromCodeCoverage]
        [TestFixture]
        public class When_Summary_Is_Called
            : Given_ReportService_Throws_Exception
        {
            private IActionResult result;

            public When_Summary_Is_Called()
            {
                result = _controller.Summary("ReportError");
            }

            [Test]
            public void Then_Result_Is_BadRequestResult()
            {
                Assert
                    .IsInstanceOf<BadRequestResult>(result);
            }
        }
    }
}