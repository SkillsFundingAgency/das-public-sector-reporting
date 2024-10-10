using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.PSRService.Web.Configuration.Authorization;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests.Authorized_For_Edit_But_Not_Submit;

public abstract class And_User_Is_Authorized_For_Edit_But_Not_Submit : Given_Home_Controller
{
    protected override void Given()
    {
        base.Given();
        AuthorizationServiceMock.Setup(m => m.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                PolicyNames.CanEditReport))
            .Returns(Task.FromResult(AuthorizationResult.Success()));
        AuthorizationServiceMock.Setup(m => m.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                PolicyNames.CanSubmitReport))
            .Returns(Task.FromResult(AuthorizationResult.Failed()));
    }
}