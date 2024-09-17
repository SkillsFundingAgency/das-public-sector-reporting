using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Moq;
using SFA.DAS.PSRService.Web.Configuration.Authorization;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Edit.Given_Report_Can_Be_Edited.And_User_Can_Submit;

public abstract class And_User_Can_Submit : Given_Report_Can_Be_Edited
{
    protected override void Given()
    {
        base.Given();

        MockAuthorizationService
            .Setup(m => m.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                PolicyNames.CanSubmitReport))
            .Returns(Task.FromResult(AuthorizationResult.Success()));

        MockAuthorizationService
            .Setup(m => m.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                PolicyNames.CanEditReport))
            .Returns(Task.FromResult(AuthorizationResult.Success()));
    }
}