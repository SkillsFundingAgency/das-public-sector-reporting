using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Summary.Given_A_Valid_Submitted_Report.
    And_User_Can_Submit
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class When_Summary_Is_Called
        : And_User_Can_Submit
    {
        private IActionResult result;
        private ReportViewModel model;

        protected override void When()
        {
            var hashedAccountId = "ABC123";
            result = _controller.Summary(hashedAccountId, "1718");

            var viewResult = result as ViewResult;

            model = viewResult?.Model as ReportViewModel;
        }

        [Test]
        public void Then_ViewModel_UserCanSubmitReports_Is_True()
        {
            var reportViewModel = ((ViewResult) result).Model as ReportViewModel;

            Assert
                .IsTrue(reportViewModel.UserCanSubmitReports);
        }

        [Test]
        public void Then_ViewModel_UserCanEditReports_Is_True()
        {
            model
                .UserCanEditReports
                .Should()
                .BeTrue();
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
            model
                .Report
                .Should()
                .NotBeNull();
        }

        [Test]
        public void Then_ViewModel_IsReadOnly_Is_False()
        {
            model
                .IsReadOnly
                .Should()
                .BeFalse();
        }
    }
}
