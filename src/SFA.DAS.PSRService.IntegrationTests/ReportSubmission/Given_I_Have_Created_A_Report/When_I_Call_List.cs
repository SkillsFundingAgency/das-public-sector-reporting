using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report
{
    public sealed class When_I_Call_List
        : Given_I_Have_Created_A_Report
    {
        private IActionResult response;

        public When_I_Call_List() : base(false){}

        protected override void When()
        {
            var hashedAccountId = "ABC123";
            response = SUT.List(hashedAccountId);
        }

        [Test]
        public void Then_Response_Is_Not_Null()
        {
            Assert.IsNotNull(response);
        }

        [Test]
        public void Then_Response_Is_A_ViewResult()
        {
            Assert.IsInstanceOf<ViewResult>(response);
        }

        [Test]
        public void Then_Response_Model_Is_A_ReportListViewModel()
        {
            Assert.IsInstanceOf<ReportListViewModel>(((ViewResult) response).Model);
        }

        [Test]
        public void Then_There_Are_No_Submitted_Reports()
        {
            var model = ((ViewResult) response).Model as ReportListViewModel;

            Assert.IsEmpty(model.SubmittedReports);
        }
    }
}