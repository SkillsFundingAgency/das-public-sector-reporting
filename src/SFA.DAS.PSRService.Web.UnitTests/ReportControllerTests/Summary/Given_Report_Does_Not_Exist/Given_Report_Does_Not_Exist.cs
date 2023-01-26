using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Summary.Given_Report_Does_Not_Exist
{
    [ExcludeFromCodeCoverage]
    public class Given_Report_Does_Not_Exist
    : Given_A_ReportController
    {
        protected override void Given()
        {
            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns((Report)null);
        }
    }
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class When_Summary_Is_Called
        : Given_Report_Does_Not_Exist
    {
        private IActionResult result;

        protected override void When()
        {
            var hashedAccountId = "ABC123";
            result = _controller.Summary(hashedAccountId, "NoReport");
        }
    }
}