using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.PSRService.Web.Configuration.Authorization;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Summary.Given_A_Valid_Submitted_Report.And_User_Can_Submit;

[ExcludeFromCodeCoverage]
public abstract class And_User_Can_Submit : GivenAValidSubmittedReport
{
    protected override void Given()
    {
        base.Given();

        MockAuthorizationService.Setup(m => m.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                PolicyNames.CanSubmitReport))
            .Returns(Task.FromResult(AuthorizationResult.Success()));

        MockAuthorizationService.Setup(m => m.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                PolicyNames.CanEditReport))
            .Returns(Task.FromResult(AuthorizationResult.Success()));
    }
}