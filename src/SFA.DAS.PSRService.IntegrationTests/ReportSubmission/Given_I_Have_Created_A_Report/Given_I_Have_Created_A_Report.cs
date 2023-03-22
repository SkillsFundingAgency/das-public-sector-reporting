using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.IntegrationTests.Web;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.ViewModels;
using StructureMap;

namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report
{
    [ExcludeFromCodeCoverage]
    public abstract class Given_I_Have_Created_A_Report
    :GivenWhenThen<ReportController>
    {
        private static Container _container;
        protected QuestionController QuestionController;
        protected Mock<IUrlHelper> MockUrlHelper;
        private bool _IsLocalAuthority;
        public Given_I_Have_Created_A_Report(bool isLocalAuthority)
        {
            this._IsLocalAuthority = isLocalAuthority;
        }
        protected override void Given()
        {
            _container = new Container();
            _container.Configure(TestHelper.ConfigureIoc());
            MockUrlHelper = new Mock<IUrlHelper>();
            MockUrlHelper.Setup(u => u.Action(It.IsAny<UrlActionContext>())).Returns("!");

            QuestionController = _container.GetInstance<QuestionController>();

            QuestionController.Url = MockUrlHelper.Object;

            SUT = _container.GetInstance<ReportController>();
            SUT.Url = MockUrlHelper.Object;
            SUT.ObjectValidator = Mock.Of<IObjectModelValidator>();

            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(c => c.User).Returns(new TestPrincipal());

            SUT.ControllerContext.HttpContext = mockContext.Object;
            QuestionController.ControllerContext.HttpContext = mockContext.Object;
            TestHelper.ClearData();

            SUT.PostIsLocalAuthority(new IsLocalAuthorityViewModel() { IsLocalAuthority = _IsLocalAuthority });
        }

        [TearDown]
        public void CleanUpAfterOurselves()
        {
            //TestHelper.ClearData();
        }
    }
}