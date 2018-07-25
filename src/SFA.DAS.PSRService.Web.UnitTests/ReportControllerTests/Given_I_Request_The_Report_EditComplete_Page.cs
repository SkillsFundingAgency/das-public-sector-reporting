using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests
{
    [TestFixture]
    public class Given_I_Request_The_Report_EditComplete_Page : ReportControllerTestBase
    {
        [Test]
        public void Then_Show_EditComplete_View()
        {
            var result = _controller.EditComplete();

            Assert.IsAssignableFrom<ViewResult>(result);
            Assert.IsTrue(((ViewResult)result).ViewName == "EditComplete");
        }
    }
}
