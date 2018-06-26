using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.DisplayText;
using SFA.DAS.PSRService.Web.ViewModels.Home;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests.View_Only_Access.Current_Report
{
    [TestFixture]
    public class When_I_Request_The_Homepage 
        : And_Current_Report_Exists
    {
        private IActionResult result;
        private ViewResult viewResult;
        private IndexViewModel model;

        protected override void When()
        {
            result = SUT.Index();
            viewResult = result as ViewResult;
            model = viewResult?.Model as IndexViewModel;
        }

        [Test]
        public void Then_ViewResult_Is_Returned()
        {
            viewResult = result as ViewResult;
        }
        [Test]
        public void Then_ViewResult_Is_No_Null()
        {
            Assert.IsNotNull(viewResult);
        }
        [Test]
        public void Then_Model_Is_An_IndexViewModel()
        {
            model = viewResult.Model as IndexViewModel;

        }
        [Test]
        public void Then_Model_Is_Not_Null()
        {
            Assert.IsNotNull(model);
        }

        [Test]
        public void Then_Create_Report_Is_Disabled()
        {
            Assert.IsFalse(model.CanCreateReport);
        }

        [Test]
        public void Then_Edit_Report_Is_Disabled()
        {
            Assert.IsFalse(model.CanEditReport);
        }

        [Test]
        public void Then_Readonly_Is_True()
        {
            model.Readonly.Should().BeTrue();
        }
        [Test]
        public void Then_CurrentReportExists_Is_True()
        {
            model.CurrentReportExists.Should().BeTrue();
        }

        [Test]
        public void Then_CurrentReportAlreadySubmitted_Is_False()
        {
            model.CurrentReportAlreadySubmitted.Should().BeFalse();
        }

        [Test]
        public void Then_The_Welcome_Message_Is_View_Only_Report_In_Progress()
        {
            var
            expectedMessage
                =
                HomePageWelcomeMessageProvider
                    .GetMesssage()
                    .ForPeriod(new Period(period))
                    .WhereUserCanOnlyView()
                    .AndReportIsInProgress();

            model
                .WelcomeMessage
                .Should()
                .BeEquivalentTo(
                    expectedMessage);
        }
    }
}
