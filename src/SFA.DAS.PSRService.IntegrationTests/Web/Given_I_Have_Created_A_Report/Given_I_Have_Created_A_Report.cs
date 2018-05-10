using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.Controllers;
using StructureMap;

namespace SFA.DAS.PSRService.IntegrationTests.Web.Given_I_Have_Created_A_Report
{
    [ExcludeFromCodeCoverage]
    public abstract class Given_I_Have_Created_A_Report
    :GivenWhenThen<ReportController>
    {
        private static Container _container;
        private QuestionController _questionController;
        private Mock<IUrlHelper> _mockUrlHelper;
        protected override void Given()
        {
            _container = new Container();
            _container.Configure(TestHelper.ConfigureIoc());
            _mockUrlHelper = new Mock<IUrlHelper>();
            _mockUrlHelper.Setup(u => u.Action(It.IsAny<UrlActionContext>())).Returns("!");

            _questionController = _container.GetInstance<QuestionController>();

            _questionController.Url = _mockUrlHelper.Object;

            SUT = _container.GetInstance<ReportController>();
            SUT.Url = _mockUrlHelper.Object;

            TestHelper.ClearData();

            SUT
                .PostCreate();
        }

        [TearDown]
        public void CleanUpAfterOurselves()
        {
            TestHelper.ClearData();
        }
    }
}