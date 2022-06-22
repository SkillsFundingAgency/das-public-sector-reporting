using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests
{
    [TestFixture]
    public class WhenActionExecuted
    {
        const string TestCommitmentsBaseUrl = "https://v2url.something";
        const string TestPublicSectorReportingBaseUrl = "https://v2url.something";
        const string TestAccountId = "XYR145";
        private HomeController _controller;
        private Mock<IUrlHelper> _mockUrlHelper;
        private Mock<IEmployerAccountService> _employeeAccountServiceMock;
        private Mock<IAuthorizationService> _authorizationServiceMock;
        private Mock<IPeriodService> _mockPeriodService;
        private IWebConfiguration _webConfiguration;

        [SetUp]
        public void SetUp()
        {
            _mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            _employeeAccountServiceMock = new Mock<IEmployerAccountService>(MockBehavior.Strict);
            _mockPeriodService = new Mock<IPeriodService>();
            _authorizationServiceMock = new Mock<IAuthorizationService>(MockBehavior.Strict);
            _webConfiguration = new WebConfiguration
            {
                EmployerCommitmentsV2BaseUrl = TestCommitmentsBaseUrl,
                RootDomainUrl = TestPublicSectorReportingBaseUrl
            };

            _employeeAccountServiceMock.Setup(x => x.GetCurrentEmployerAccountId(It.IsAny<HttpContext>())).Returns(new EmployerIdentifier { AccountId = TestAccountId });

            _controller = new HomeController(null, _employeeAccountServiceMock.Object, _webConfiguration, _mockPeriodService.Object, _authorizationServiceMock.Object) { Url = _mockUrlHelper.Object };
        }

        [Test]
        public void Then_homeurl_is_populated()
        {
            var filterMetadata = new Mock<IFilterMetadata>();
            var ctx = new ActionExecutingContext(new ActionContext(Mock.Of<HttpContext>(), new RouteData(), new ActionDescriptor()), new List<IFilterMetadata> { filterMetadata.Object }, new Dictionary<string, object> { { "action", "arg" } }, _controller);

            _controller.OnActionExecuting(ctx);

            var homeUrl = _controller.ViewData["HomeUrl"];
            Assert.AreEqual($"{TestCommitmentsBaseUrl}/{TestAccountId}", homeUrl);
        }
    }
}
