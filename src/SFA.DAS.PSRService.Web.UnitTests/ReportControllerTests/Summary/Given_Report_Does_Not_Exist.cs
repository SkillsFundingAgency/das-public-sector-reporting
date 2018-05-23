﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Summary
{
    [ExcludeFromCodeCoverage]
    public class Given_Report_Does_Not_Exist
    : Given_A_ReportController
    {
        public Given_Report_Does_Not_Exist()
        {
            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns((Report)null);
        }

        [ExcludeFromCodeCoverage]
        [TestFixture]
        public class When_Summary_Is_Called
            : Given_Report_Does_Not_Exist
        {
            private IActionResult result;

            public When_Summary_Is_Called()
            {
                result = _controller.Summary("NoReport");
            }

            [Test]
            public void Then_Result_Is_NotFoundResult()
            {
                Assert
                    .IsAssignableFrom<NotFoundResult>(result);
            }
        }
    }
}