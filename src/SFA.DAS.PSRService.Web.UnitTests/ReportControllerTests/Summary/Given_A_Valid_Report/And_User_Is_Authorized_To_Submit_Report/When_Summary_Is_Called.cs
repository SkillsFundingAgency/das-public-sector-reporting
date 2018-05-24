using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Summary.Given_A_Valid_Report.
    And_User_Is_Authorized_To_Submit_Report
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class When_Summary_Is_Called
        : And_User_Is_Authorized_To_Submit_Report
    {
        private IActionResult result;

        protected override void When()
        {
            result = _controller.Summary("1718");
        }

        [Test]
        public void Then_ViewModel_UserCanSubmitReports_Is_True()
        {
            var reportViewModel = ((ViewResult) result).Model as ReportViewModel;

            Assert
                .IsTrue(reportViewModel.UserCanSubmitReports);
        }

        [Test]
        public void Then_Result_Is_ViewResult()
        {
            Assert
                .IsNotNull(result);

            Assert
                .IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Then_ViewName_Is_Summary()
        {
            Assert
                .AreEqual("Summary", ((ViewResult) result).ViewName, "View name does not match, should be: Summary");
        }

        [Test]
        public void Then_ViewModel_Is_ReportViewModel()
        {
            Assert
                .IsNotNull(((ViewResult) result).Model);

            Assert
                .IsInstanceOf<ReportViewModel>(((ViewResult) result).Model);
        }

        [Test]
        public void Then_ViewModel_Has_Report()
        {
            var reportViewModel = ((ViewResult) result).Model as ReportViewModel;

            Assert
                .IsNotNull(reportViewModel.Report);

            Assert
                .IsNotNull(reportViewModel.Report.Id);
        }
    }
}
