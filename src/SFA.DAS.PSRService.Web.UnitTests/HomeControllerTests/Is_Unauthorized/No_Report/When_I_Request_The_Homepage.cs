﻿using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.ViewModels.Home;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests.Is_Unauthorized.No_Report
{
    [TestFixture]
    public class When_I_Request_The_Homepage : And_No_Current_Report_Exists
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
    }
}
