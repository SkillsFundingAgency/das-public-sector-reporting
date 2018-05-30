using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.Configuration.Authorization;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests.Is_Authorized
{
    [TestFixture]
    public class And_Is_Authorized : Given_Home_Controller
    {
        protected override void Given()
        {
            base.Given();
            _authorizationServiceMock.Setup(m => m.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<object>(),
                    PolicyNames.CanEditReport))
                .Returns(Task.FromResult(AuthorizationResult.Success()));
        }
    }
}
