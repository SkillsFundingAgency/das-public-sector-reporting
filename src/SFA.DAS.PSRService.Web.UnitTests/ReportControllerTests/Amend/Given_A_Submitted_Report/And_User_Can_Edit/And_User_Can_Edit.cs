using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.PSRService.Web.Configuration.Authorization;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Amend.Given_A_Submitted_Report.And_User_Can_Edit;

public class And_User_Can_Edit : GivenASubmittedReport
{
    protected override void Given()
    {
        base.Given();

        MockAuthorizationService
            .Setup(m => m.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<object>(),
                    PolicyNames.CanSubmitReport))
            .Returns(Task.FromResult(AuthorizationResult.Failed()));

        MockAuthorizationService
            .Setup(m => m.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<object>(),
                    PolicyNames.CanEditReport))
            .Returns(Task.FromResult(AuthorizationResult.Success()));
    }
}