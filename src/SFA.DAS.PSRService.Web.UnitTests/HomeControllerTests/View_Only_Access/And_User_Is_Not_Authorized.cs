using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Moq;
using SFA.DAS.PSRService.Web.Configuration.Authorization;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests.View_Only_Access
{
    public abstract class And_User_Is_Not_Authorized : Given_Home_Controller
    {
        protected override void Given()
        {
            base.Given();
            _authorizationServiceMock.Setup(m => m.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<object>(),
                    PolicyNames.CanEditReport))
                .Returns(Task.FromResult(AuthorizationResult.Failed()));
            _authorizationServiceMock.Setup(m => m.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<object>(),
                    PolicyNames.CanSubmitReport))
                .Returns(Task.FromResult(AuthorizationResult.Failed()));
        }
    }
}
