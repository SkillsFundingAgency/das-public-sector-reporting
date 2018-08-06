using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Moq;
using SFA.DAS.PSRService.Web.Configuration.Authorization;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests.Authorized_For_Submit
{
    public abstract class And_User_Is_Authorized_For_Submit : Given_Home_Controller
    {
        protected override void Given()
        {
            base.Given();
            _authorizationServiceMock.Setup(m => m.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<object>(),
                    PolicyNames.CanSubmitReport))
                .Returns(Task.FromResult(AuthorizationResult.Success()));
            _authorizationServiceMock.Setup(m => m.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<object>(),
                    PolicyNames.CanEditReport))
                .Returns(Task.FromResult(AuthorizationResult.Success()));
        }
    }
}
