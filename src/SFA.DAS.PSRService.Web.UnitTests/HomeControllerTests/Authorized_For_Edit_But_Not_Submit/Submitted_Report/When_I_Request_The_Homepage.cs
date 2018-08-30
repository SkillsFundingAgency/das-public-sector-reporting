using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.DisplayText;
using SFA.DAS.PSRService.Web.ViewModels.Home;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests.Authorized_For_Edit_But_Not_Submit.Submitted_Report
{
    [TestFixture]
    public class When_I_Request_The_Homepage : And_Current_Report_Submitted
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
        public void Then_Report_Period_Matches_Current()
        {
            Assert.AreEqual(CurrentPeriod, model.Period.PeriodString);
        }

        [Test]
        public void Then_Readonly_Is_False()
        {
            model.Readonly.Should().BeFalse();
        }

        [Test]
        public void Then_CurrentReportAlreadySubmitted_Is_True()
        {
            model.CurrentReportAlreadySubmitted.Should().BeTrue();
        }

        [Test]
        public void Then_The_Welcome_Message_Is_Edit_Report_Submitted()
        {
            var
                expectedMessage
                    =
                    HomePageWelcomeMessageProvider
                        .GetMesssage()
                        .ForPeriod(CurrentPeriod)
                        .WhereUserCanEdit()
                        .AndReportIsAlreadySubmitted();

            model
                .WelcomeMessage
                .Should()
                .BeEquivalentTo(
                    expectedMessage);
        }
    }
}
