using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.DisplayText;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Summary.Given_A_Valid_Not_Submitted_Report.
    And_User_Has_View_Only_Access
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class When_Summary_Is_Called
        : And_User_Has_View_Only_Access
    {
        private IActionResult result;
        private ReportViewModel model;

        protected override void When()
        {
            result = _controller.Summary("1718");

            var viewResult = result as ViewResult;

            model = viewResult?.Model as ReportViewModel;
        }

        [Test]
        public void Then_ViewModel_UserCanSubmitReports_Is_False()
        {
            Assert
                .IsFalse(model.UserCanSubmitReports);
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

        [Test]
        public void Then_Aubtitle_Is_Appropriate_For_User_With_ViewOnly_Access()
        {
            var
                expectedText
                    =
                    SummaryPageMessageBuilder
                        .GetSubtitle()
                        .ForViewOnlyUser();

            model
                .Subtitle
                .Should()
                .BeEquivalentTo(
                    expectedText);
        }
    }
}
